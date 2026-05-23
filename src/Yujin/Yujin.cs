using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Yujin : Bot
{
    bool hasTarget = false;

    int lostTargetTicks = 0;
    int fireCooldown = 0;

    bool dodgeDirection = true;
    int dodgeTicks = 0;

    // jarak aman dari tembok
    const double WALL_MARGIN = 120;


    // mulai bot
    static void Main(string[] args)
    {
        new Yujin().Start();
    }

    // ambil config bot
    Yujin()
        : base(BotInfo.FromFile("Yujin.json"))
    {
    }


    // round mulai -> setup awal
    public override void Run()
    {
        BodyColor = Color.DarkRed;
        TurretColor = Color.Black;
        RadarColor = Color.Red;
        BulletColor = Color.Orange;
        ScanColor = Color.Yellow;
        TracksColor = Color.DarkGray;
        GunColor = Color.Black;

        // radar + gun bebas muter
        AdjustGunForBodyTurn = true;
        AdjustRadarForGunTurn = true;
        AdjustRadarForBodyTurn = true;

        while (IsRunning)
        {
            // cooldown tembak
            if (fireCooldown > 0)
                fireCooldown--;

            // timer dodge
            if (dodgeTicks > 0)
                dodgeTicks--;

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
                SetTurnRadarRight(360);

                // jalan santai
                SetForward(80);
                SetTurnRight(20);
            }

            Go();
        }
    }


    // ada musuh -> lock + gerak + tembak
    public override void OnScannedBot(
        ScannedBotEvent e)
    {
        hasTarget = true;
        lostTargetTicks = 0;

        double distance =
            DistanceTo(
                e.X,
                e.Y
            );


        // arahkan senjata ke musuh
        double gunBearing =
            GunBearingTo(
                e.X,
                e.Y
            );

        SetTurnGunLeft(gunBearing);


        // radar bakal ngikut target
        double radarBearing =
            RadarBearingTo(
                e.X,
                e.Y
            );

        SetTurnRadarLeft(radarBearing);


        double bearing =
            BearingTo(
                e.X,
                e.Y
            );


        // cek deket tembok apa gak
        bool nearWall =
            X < WALL_MARGIN ||
            X > ArenaWidth - WALL_MARGIN ||
            Y < WALL_MARGIN ||
            Y > ArenaHeight - WALL_MARGIN;


        // deket tembok -> kabur
        if (nearWall)
        {
            SetTurnRight(60);

            if (dodgeDirection)
                SetForward(250);

            else
                SetBack(250);
        }


        // gerak normal sambil dodge
        else
        {
            // body nyamping dari target
            SetTurnLeft(
                (bearing - 90) * 0.5
            );


            // ganti arah dodge
            if (dodgeTicks == 0)
            {
                dodgeDirection =
                    !dodgeDirection;

                dodgeTicks = 18;
            }


            if (dodgeDirection)
                SetForward(220);

            else
                SetBack(220);
        }


        // aim pas + cooldown habis -> tembak
        if (
            GunHeat == 0 &&
            fireCooldown == 0 &&
            Math.Abs(gunBearing) < 5
        )
        {
            if (distance < 80)
                Fire(3);

            else if (distance < 200)
                Fire(2);

            else
                Fire(1);

            fireCooldown = 10;
        }

        Go();
    }


    // nabrak tembok -> balik arah
    public override void OnHitWall(
        HitWallEvent e)
    {
        dodgeDirection =
            !dodgeDirection;

        SetBack(250);
        SetTurnRight(140);

        Go();
    }


    // kena peluru -> dodge agresif
    public override void OnHitByBullet(
        HitByBulletEvent e)
    {
        dodgeDirection =
            !dodgeDirection;

        dodgeTicks = 12;

        SetTurnRight(70);

        if (dodgeDirection)
            SetForward(180);

        else
            SetBack(180);

        Go();
    }


    // target mati -> cari baru
    public override void OnBotDeath(
        BotDeathEvent e)
    {
        hasTarget = false;
    }
}