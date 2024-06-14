using UnityEngine;

public static class GameEnums 
{
    public enum GameEvents
    {
        PlayerOnCollidedGround = 1,
        PlayerOnCollidedPipe = 2,
        PlayerOnLose = 3,
        PlayerOnPass = 4,
        PipeOnReceiveNewPos = 5,
        OnPlaySfx = 6,
        GameOverOnPopUp = 7,
        WhiteFrameOnPopUp = 8,
        ScoreBoardOnPopUp = 9,
        ScoreBoardOnUpdateScore = 10,
        UIButtonOnPopUp = 11,
        ButtonStartOnClick = 12,
        GameOverOnAllowPopUp = 13,
        GameOnStart = 14, //Táp vào thì bắt đầu chơi (enable grav)
        ParticleOnPopUp = 15,
        NewOnPopUp = 16,
    }

    public enum EPoolable
    {
        PipeControl = 0,
        Particle = 1,
    }

    public enum ESfx
    {
        Flap = 1,
        Passed = 2,
        Collided = 3,
        Die = 4,
        Transition = 5
    }

    public enum ESpecialKeys
    {
        HighestScore = 0,
    }

    public enum EMedal
    {
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Platinum = 4,
    }
}
