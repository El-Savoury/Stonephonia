﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Managers;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    public class LoseScreen : Screen
    {
        Timer mRoomTimer;
        Texture2D mDefaultbg;
        CutsceneSprite mFairy, mPlayerDeath, mPlayer;
        Vector2 mFairyPos = new Vector2(600, 300);
        Vector2 mPlayerDeathPos = new Vector2(ScreenManager.pusher.mPosition.X - 15, ScreenManager.pusher.mPosition.Y);
        Vector2 mPlayerPos = new Vector2(20, 452);
        Texture2D mDeathTexture;
        Fader mText;
        FaderManager mTextFader;
        ScreenTransition mScreenTransition;
        float mBlackSquareAlpha = 1.0f;

        int fairySpawn = 3;
        int fairyDespawn = 23;
        int deathDespawn = 14;
        int textSpawn = 7;
        int textOnScreen = 5;
        int playerSpawn = 18;
        int playerStopTime = 1000000;
        int changeScreen = 27;

        public LoseScreen()
        {
        }

        public override void LoadAssets()
        {
            mRoomTimer = new Timer();
            mScreenTransition = new ScreenTransition();
            mDefaultbg = ScreenManager.contentMgr.Load<Texture2D>("Sprites/default_bg");

            if (ScreenManager.pusher.mDirection) { mDeathTexture = ScreenManager.contentMgr.Load<Texture2D>("Sprites/death_right"); }
            else { mDeathTexture = ScreenManager.contentMgr.Load<Texture2D>("Sprites/death_left"); }

            mFairy = new CutsceneSprite(mFairyPos, fairySpawn, fairyDespawn, CutsceneSprite.State.inactive,
                new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_sheet"),
                new Point(128, 128), new Point(0, 0), new Point(4, 1), 200, Color.White, false),
                new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_fader"), new Vector2(mFairyPos.X, mFairyPos.Y + 4), Color.White));

            mPlayerDeath = new CutsceneSprite(mPlayerDeathPos, 0, deathDespawn, CutsceneSprite.State.activated,
                new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_death"),
                new Point(100, 84), new Point(3, ScreenManager.pusher.mDirection ? 0 : 1), new Point(4, 2), 1000, Color.White, true),
                new Fader(mDeathTexture, new Vector2(mPlayerDeathPos.X - 7, mPlayerDeathPos.Y), Color.White));

            mPlayer = new CutsceneSprite(mPlayerPos, playerSpawn, playerStopTime, CutsceneSprite.State.inactive,
                new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_stage_one"),
                new Point(60, 84), new Point(0, 0), new Point(2, 1), 200, Color.White, false),
                new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_fader"), mPlayerPos, Color.White));

            mText = new Fader(ScreenManager.font, "\"Try again\"", new Vector2(0, 600), Colours.lightBlue, 0.0f);
            mTextFader = new FaderManager(new Fader[1] { mText });
        }

        public override void UnloadAssests()
        {
        }

        private void GotoGameplayScreen(float timeLimit)
        {
            float fadeIn = 0.008f;
            float fadeOut = 0.04f;

            if (mRoomTimer.mCurrentTime > timeLimit)
            {
                mScreenTransition.FadeToGamePlay(fadeOut, fadeIn, this);
                ScreenManager.pusher.Reset();
                if (!mScreenTransition.mFadingIn) { mBlackSquareAlpha = 0.0f; }
            }
        }

        public override void Update(GameTime gameTime)
        {
            mRoomTimer.Update(gameTime);
            SoundManager.StopMusic();
            mFairy.Update(gameTime, true);
            mPlayerDeath.Update(gameTime, false);
            mPlayer.Update(gameTime, true);
            mTextFader.FadeInAndOut(mText, 0.02f, 0.03f, textSpawn, textOnScreen);
            mTextFader.Update(gameTime);
            GotoGameplayScreen(changeScreen);
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mDefaultbg, Vector2.Zero, Color.White);
            spriteBatch.Draw(ScreenManager.blackSquare, Vector2.Zero, Color.White * mBlackSquareAlpha);
            mFairy.Draw(spriteBatch);
            mPlayerDeath.Draw(spriteBatch);
            mTextFader.DrawString(spriteBatch);
            mScreenTransition.Draw(spriteBatch);
            mPlayer.Draw(spriteBatch);
        }
    }
}
