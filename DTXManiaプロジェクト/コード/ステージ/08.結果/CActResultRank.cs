﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CActResultRank : CActivity
    {
        // コンストラクタ

        public CActResultRank()
        {
            base.bNotActivated = true;
        }


        // メソッド

        public void tアニメを完了させる()
        {
            this.ctランク表示.n現在の値 = this.ctランク表示.n終了値;
        }


        // CActivity 実装

        public override void OnActivate()
        {
            #region [ 本体位置 ]

            int n中X = 480;
            int n中Y = 50;

            int n左X = 300;
            int n左Y = -15;

            int n右X = 720;
            int n右Y = -15;

            this.n本体X[0] = 0;
            this.n本体Y[0] = 0;

            this.n本体X[1] = 0;
            this.n本体Y[1] = 0;

            this.n本体X[2] = 0;
            this.n本体Y[2] = 0;

            if (CDTXMania.ConfigIni.bDrums有効)
            {
                this.n本体X[0] = n中X;
                this.n本体Y[0] = n中Y;
            }
            else if (CDTXMania.ConfigIni.bGuitar有効)
            {
                if (!CDTXMania.DTX.bチップがある.Bass)
                {
                    this.n本体X[1] = n中X;
                    this.n本体Y[1] = n中Y;
                }
                else if (!CDTXMania.DTX.bチップがある.Guitar)
                {
                    this.n本体X[2] = n中X;
                    this.n本体Y[2] = n中Y;
                }
                else if (CDTXMania.ConfigIni.bIsSwappedGuitarBass)
                {
                    this.n本体X[1] = n右X;
                    this.n本体Y[1] = n右Y;
                    this.n本体X[2] = n左X;
                    this.n本体Y[2] = n左Y;
                }
                else
                {
                    this.n本体X[1] = n左X;
                    this.n本体Y[1] = n左Y;
                    this.n本体X[2] = n右X;
                    this.n本体Y[2] = n右Y;
                }
            }
            #endregion

            this.b全オート.Drums = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay;
            this.b全オート.Guitar = CDTXMania.ConfigIni.bギターが全部オートプレイである;
            this.b全オート.Bass = CDTXMania.ConfigIni.bベースが全部オートプレイである;

            base.OnActivate();
        }
        public override void OnDeactivate()
        {
            if (this.ctランク表示 != null)
            {
                this.ctランク表示 = null;
            }
            base.OnDeactivate();
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {

                this.txStageCleared = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\ScreenResult StageCleared.png"));
                this.txFullCombo = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\ScreenResult fullcombo.png"));
                this.txExcellent = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\ScreenResult Excellent.png"));

                for (int j = 0; j < 3; j++)
                {
                    switch (CDTXMania.stage結果.nRankValue[j])
                    {
                        case 0:
                            this.txランク文字[j] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_rankSS.png"));
                            break;

                        case 1:
                            this.txランク文字[j] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_rankS.png"));
                            break;

                        case 2:
                            this.txランク文字[j] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_rankA.png"));
                            break;

                        case 3:
                            this.txランク文字[j] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_rankB.png"));
                            break;

                        case 4:
                            this.txランク文字[j] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_rankC.png"));
                            break;

                        case 5:
                            this.txランク文字[j] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_rankD.png"));
                            break;

                        case 6:
                        case 99:	// #23534 2010.10.28 yyagi: 演奏チップが0個のときは、rankEと見なす
                            this.txランク文字[j] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_rankE.png"));
                            if (this.b全オート[j])
                                this.txランク文字[j] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_rankSS.png"));
                            break;

                        default:
                            this.txランク文字[j] = null;
                            break;
                    }
                }
                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                CDTXMania.tReleaseTexture(ref this.txStageCleared);
                CDTXMania.tReleaseTexture(ref this.txFullCombo);
                CDTXMania.tReleaseTexture(ref this.txExcellent);
                CDTXMania.t安全にDisposeする(ref this.txランク文字);
                base.OnManagedReleaseResources();
            }
        }
        public override int On進行描画()
        {
            if (base.bNotActivated)
            {
                return 0;
            }
            if (base.b初めての進行描画)
            {
                this.ctランク表示 = new CCounter(0, 500, 1, CDTXMania.Timer);
                base.b初めての進行描画 = false;
            }
            this.ctランク表示.t進行();

            for (int j = 0; j < 3; j++)
            {
                if (this.n本体X[j] != 0)
                {
                    #region [ ランク文字 ]
                    if (this.txランク文字[j] != null)
                    {
                        double num2 = ((double)this.ctランク表示.n現在の値 - 200.0) / 300.0;

                        if (this.ctランク表示.n現在の値 >= 200.0)
                            this.txランク文字[j].tDraw2D(CDTXMania.app.Device, this.n本体X[j], this.n本体Y[j] + ((int)((double)this.txランク文字[j].sz画像サイズ.Height * (1.0 - num2))), new Rectangle(0, 0, txランク文字[j].sz画像サイズ.Width, (int)((double)this.txランク文字[j].sz画像サイズ.Height * num2)));
                    }
                    #endregion

                    #region [ フルコンボ ]
                    int num14 = -165 + this.n本体X[j];
                    int num15 = 100 + this.n本体Y[j];

                    if (CDTXMania.stage結果.stPerformanceEntry[j].nPerfect数 == CDTXMania.stage結果.stPerformanceEntry[j].n全チップ数)
                    {
                        if (this.txExcellent != null)
                            this.txExcellent.tDraw2D(CDTXMania.app.Device, num14, num15);
                    }
                    else if (CDTXMania.stage結果.stPerformanceEntry[j].bIsFullCombo)
                    {
                        if (this.txFullCombo != null)
                            this.txFullCombo.tDraw2D(CDTXMania.app.Device, num14, num15);
                    }
                    else
                    {
                        if (this.txStageCleared != null)
                            this.txStageCleared.tDraw2D(CDTXMania.app.Device, num14, num15);
                    }
                    #endregion
                }
            }

            if (!this.ctランク表示.bReachedEndValue)
            {
                return 0;
            }
            return 1;
        }


        // その他

        #region [ private ]
        //-----------------
        private CCounter ctランク表示;
        private STDGBVALUE<int> n本体X;
        private STDGBVALUE<int> n本体Y;
        private STDGBVALUE<bool> b全オート;
        private STDGBVALUE<CTexture> txランク文字;
        private CTexture txStageCleared;
        private CTexture txFullCombo;
        private CTexture txExcellent;
        //-----------------
        #endregion
    }
}
