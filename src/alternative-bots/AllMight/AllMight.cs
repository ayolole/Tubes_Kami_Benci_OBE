using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class AllMight : Bot
{
    Random rnd = new Random();

    bool hasTarget = false;
    int lostTargetTicks = 0;

    double moveAmount;
    double targetX = 0;
    double targetY = 0;
    double targetDistance = double.MaxValue;

    int targetTimeout = 0;

    // mulai bot
    static void Main()
    {
        new AllMight().Start();
    }

    // ambil config bot
    AllMight()
        : base(BotInfo.FromFile("AllMight.json"))
    {
    }

    // round mulai -> setup awal
    public override void Run()
    {
        AdjustGunForBodyTurn = true;
        AdjustRadarForGunTurn = true;
        AdjustRadarForBodyTurn = true;

        moveAmount = Math.Max(ArenaWidth, ArenaHeight);

        // cari dinding dulu
        TurnLeft(Direction % 90);
        Forward(moveAmount);
        TurnRight(90);

        while (IsRunning)
        {
            lostTargetTicks++;

            // target ilang -> reset
            if (lostTargetTicks > 10)
                hasTarget = false;

            // gak ada target -> muter radar
            if (!hasTarget)
                SetTurnRadarRight(360);

            // jalan pinggir map
            SetForward(moveAmount);
            SetTurnRight(90);

            Go();
        }

        // target lama gak update -> buang
        targetTimeout++;

        if (targetTimeout > 40)
        {
            hasTarget = false;
            targetDistance = double.MaxValue;
        }
    }

    // ada musuh -> lock + aim + tembak
    public override void OnScannedBot(ScannedBotEvent e)
    {
        double distance = DistanceTo(e.X, e.Y);

        // pilih musuh paling deket
        if (!hasTarget ||
            distance < targetDistance ||
            targetTimeout > 20)
        {
            targetX = e.X;
            targetY = e.Y;

            targetDistance = distance;

            targetTimeout = 0;
            hasTarget = true;
        }

        targetTimeout++;

        // ada target lebih penting -> skip
        if (DistanceTo(targetX, targetY) < distance - 20)
            return;

        // radar bakal ngikut target
        double radarBearing =
            RadarBearingTo(targetX, targetY);

        SetTurnRadarLeft(radarBearing * 2);

        // nebak posisi target nanti
        double futureX =
            targetX +
            Math.Sin(
                e.Direction *
                Math.PI / 180
            )
            *
            e.Speed *
            4;

        double futureY =
            targetY +
            Math.Cos(
                e.Direction *
                Math.PI / 180
            )
            *
            e.Speed *
            4;

        // arahkan senjata ke prediksi
        double gunBearing =
            GunBearingTo(
                futureX,
                futureY
            );

        SetTurnGunLeft(gunBearing);

        // musuh deket -> mundur
        if (distance < 120)
        {
            SetBack(60);
            SetTurnRight(90);
        }

        // atur sakitnya peluru
        double firePower =
            distance < 80 ? 3 :
            distance < 180 ? 2 :
            1;

        // aim pas -> tembak
        if (GunHeat == 0 &&
            Math.Abs(gunBearing) < 6)
        {
            Fire(firePower);
        }

        Go();
    }

    // kena peluru -> ngindar random
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        Back(40 + rnd.Next(30));

        if (rnd.Next(2) == 0)
            TurnRight(90);
        else
            TurnLeft(90);
    }

    // nabrak tembok -> mundur
    public override void OnHitWall(HitWallEvent e)
    {
        Back(30);
        TurnRight(90);
    }

    // nabrak bot -> menjauh
    public override void OnHitBot(HitBotEvent e)
    {
        Back(80);
        TurnRight(90);
    }
}