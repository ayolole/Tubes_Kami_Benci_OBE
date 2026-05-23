using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Deku : Bot
{
    bool movingForward = true;
    bool targetLocked = false;

    int lostTargetTicks = 0;
    const int MAX_LOST = 25;

    const double WALL_MARGIN = 120;

    Random rnd = new Random();

    // mulai bot
    static void Main()
    {
        new Deku().Start();
    }

    // ambil config bot
    Deku()
        : base(BotInfo.FromFile("Deku.json"))
    {
    }

    // round mulai -> setup awal
    public override void Run()
    {
        BodyColor = Color.Red;
        GunColor = Color.Black;
        RadarColor = Color.Yellow;
        BulletColor = Color.Orange;
        ScanColor = Color.Cyan;

        while (IsRunning)
        {
            // target ilang lama -> reset
            if (targetLocked)
            {
                lostTargetTicks++;

                if (lostTargetTicks > MAX_LOST)
                    targetLocked = false;
            }

            // deket tembok -> balik ke tengah
            bool nearWall =
                X < WALL_MARGIN ||
                X > ArenaWidth - WALL_MARGIN ||
                Y < WALL_MARGIN ||
                Y > ArenaHeight - WALL_MARGIN;

            if (nearWall)
            {
                double centerX =
                    ArenaWidth / 2;

                double centerY =
                    ArenaHeight / 2;

                double bearing =
                    BearingTo(
                        centerX,
                        centerY
                    );

                SetTurnLeft(bearing);
                SetForward(250);

                targetLocked = false;
            }

            // gerak normal
            else
            {
                SetForward(40000);

                int turn =
                    rnd.Next(
                        10,
                        25
                    );

                if (movingForward)
                    SetTurnRight(turn);

                else
                    SetTurnLeft(turn);
            }

            // gak ada target -> muter cari
            if (!targetLocked)
            {
                SetTurnGunRight(180);
            }

            Go();
        }
    }

    // ada musuh -> lock + aim + tembak
    public override void OnScannedBot(
        ScannedBotEvent e)
    {
        targetLocked = true;
        lostTargetTicks = 0;

        double distance =
            DistanceTo(
                e.X,
                e.Y
            );

        // atur power peluru
        double firePower =
            distance < 120 ? 3 :
            distance < 250 ? 2 :
            1;

        double bulletSpeed =
            20 - (3 * firePower);

        double time =
            distance /
            bulletSpeed;

        // nebak posisi target nanti
        double futureX =
            e.X +
            Math.Sin(
                e.Direction *
                Math.PI / 180
            )
            *
            e.Speed *
            time;

        double futureY =
            e.Y +
            Math.Cos(
                e.Direction *
                Math.PI / 180
            )
            *
            e.Speed *
            time;

        double gunBearing =
            GunBearingTo(
                futureX,
                futureY
            );

        // arahkan senjata ke prediksi
        SetTurnGunRight(
            gunBearing * 1.2
        );

        // aim pas -> tembak
        if (
            Math.Abs(gunBearing) < 2 &&
            GunHeat == 0
        )
        {
            Fire(firePower);
        }
    }

    // nabrak tembok -> balik arah
    public override void OnHitWall(
        HitWallEvent e)
    {
        ReverseDirection();

        targetLocked = false;
    }

    // nabrak bot -> tembak terus kabur
    public override void OnHitBot(
        HitBotEvent e)
    {
        Fire(3);

        ReverseDirection();

        SetTurnRight(90);

        targetLocked = false;
    }
    // kena peluru -> ngindar
    public override void OnHitByBullet(
        HitByBulletEvent e)
    {
        ReverseDirection();

        SetTurnRight(
            rnd.Next(
                30,
                70
            )
        );

        Go();
    }

    // ganti arah maju mundur
    void ReverseDirection()
    {
        if (movingForward)
        {
            SetBack(40000);

            movingForward = false;
        }

        else
        {
            SetForward(40000);

            movingForward = true;
        }
    }
}