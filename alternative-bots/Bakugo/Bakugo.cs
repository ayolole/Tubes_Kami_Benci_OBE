using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Bakugo : Bot
{
    bool hasTarget = false;

    int lostTargetTicks = 0;
    int fireCooldown = 0;

    // mulai bot
    static void Main(string[] args)
    {
        new Bakugo().Start();
    }

    // ambil config bot
    Bakugo()
        : base(BotInfo.FromFile("Bakugo.json"))
    {
    }

    // round mulai -> setup awal
    public override void Run()
    {
        BodyColor = Color.Blue;
        GunColor = Color.Black;
        RadarColor = Color.Red;
        BulletColor = Color.Yellow;

        // radar + gun nyatu sama body
        AdjustGunForBodyTurn = false;
        AdjustRadarForGunTurn = false;
        AdjustRadarForBodyTurn = false;

        while (IsRunning)
        {
            if (fireCooldown > 0)
                fireCooldown--;

            // target ilang lama -> reset
            if (hasTarget)
            {
                lostTargetTicks++;

                if (lostTargetTicks > 25)
                    hasTarget = false;
            }

            // gak ada target -> muter cari
            if (!hasTarget)
            {
                TurnRight(30);
            }
        }
    }


    // ada musuh -> ikutin + jaga jarak + tembak
    public override void OnScannedBot(ScannedBotEvent e)
    {
        hasTarget = true;
        lostTargetTicks = 0;

        double distance =
            DistanceTo(e.X, e.Y);

        double bearing =
            BearingTo(e.X, e.Y);

        // belok dikit ngikut target
        SetTurnLeft(bearing * 0.6);


        // musuh jauh -> maju
        if (distance > 220)
        {
            SetForward(80);
        }

        else if (distance > 120)
        {
            SetForward(40);
        }

        // musuh deket -> mundur
        else if (distance < 80)
        {
            SetBack(30);
        }

        else
        {
            SetForward(10);
        }


        // aim pas + gak panas -> tembak
        if (
            GunHeat == 0 &&
            fireCooldown == 0 &&
            Math.Abs(bearing) < 15
        )
        {
            if (distance < 150)
                Fire(2);
            else
                Fire(1);

            fireCooldown = 10;
        }

        Go();
    }


    // kena peluru -> ngindar dikit
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        SetTurnRight(20);
        SetForward(40);

        Go();
    }


    // nabrak tembok -> mundur terus belok
    public override void OnHitWall(HitWallEvent e)
    {
        SetBack(100);
        SetTurnRight(120);

        Go();
    }


    // target mati -> cari target baru
    public override void OnBotDeath(BotDeathEvent e)
    {
        hasTarget = false;
    }
}