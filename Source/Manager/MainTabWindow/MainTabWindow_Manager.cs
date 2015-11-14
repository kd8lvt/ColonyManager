﻿// Manager/MainTabWindow_Manager.cs
// 
// Copyright Karel Kroeze, 2015.
// 
// Created 2015-11-04 19:24

using RimWorld;
using UnityEngine;
using Verse;

namespace FM
{
    internal class MainTabWindow_Manager : MainTabWindow
    {
        public static ManagerTab CurrentTab;

        private static float _iconSize = 30f;
        private static float _margin = Utilities.Margin;
        public ManagerTab DefaultTab = Manager.Get.ManagerTabs[0];

        public MainTabWindow_Manager()
        {
            if ( CurrentTab == null )
            {
                CurrentTab = DefaultTab;
            }
        }

        public static void GoTo( ManagerTab tab, ManagerJob job = null )
        {
            // call pre/post open/close methods
            ManagerTab old = CurrentTab;
            old.PreClose();
            tab.PreOpen();
            CurrentTab = tab;
            old.PostClose();
            tab.PostOpen();

            // if desired, set selected.
            if ( job != null )
            {
                tab.Selected = job;
            }
        }

        public override void DoWindowContents( Rect canvas )
        {
            // three areas of icons for tabs, left middle and right.
            var leftIcons = new Rect( 0f, 0f, _margin + Manager.Get.ManagerTabsLeft.Count * ( _iconSize + _margin ),
                                      _iconSize );
            var middleIcons = new Rect( 0f, 0f, _margin + Manager.Get.ManagerTabsMiddle.Count * ( _iconSize + _margin ),
                                        _iconSize );
            var rightIcons = new Rect( 0f, 0f, _margin + Manager.Get.ManagerTabsRight.Count * ( _iconSize + _margin ),
                                       _iconSize );

            // finetune rects
            middleIcons = middleIcons.CenteredOnXIn( canvas );
            rightIcons.x += canvas.width - rightIcons.width;

            // left icons (probably only overview, but hey...)
            GUI.BeginGroup( leftIcons );
            var cur = new Vector2( _margin, 0f );
            foreach ( ManagerTab tab in Manager.Get.ManagerTabsLeft )
            {
                var iconRect = new Rect( cur.x, cur.y, _iconSize, _iconSize );
                DrawTabIcon( iconRect, tab );
                cur.x += _iconSize + _margin;
            }
            GUI.EndGroup();

            // middle icons (the bulk of icons)
            GUI.BeginGroup( middleIcons );
            cur = new Vector2( _margin, 0f );
            foreach ( ManagerTab tab in Manager.Get.ManagerTabsMiddle )
            {
                var iconRect = new Rect( cur.x, cur.y, _iconSize, _iconSize );
                DrawTabIcon( iconRect, tab );
                cur.x += _iconSize + _margin;
            }
            GUI.EndGroup();

            // right icons (probably only import/export, possbile settings?)
            GUI.BeginGroup( rightIcons );
            cur = new Vector2( _margin, 0f );
            foreach ( ManagerTab tab in Manager.Get.ManagerTabsRight )
            {
                var iconRect = new Rect( cur.x, cur.y, _iconSize, _iconSize );
                DrawTabIcon( iconRect, tab );
                cur.x += _iconSize + _margin;
            }
            GUI.EndGroup();

            // delegate actual content to the specific manager.
            var contentCanvas = new Rect( 0f, _iconSize + _margin, canvas.width, canvas.height - _iconSize - _margin );
            GUI.BeginGroup( contentCanvas );
            CurrentTab.DoWindowContents( contentCanvas );
            GUI.EndGroup();
        }

        public void DrawTabIcon( Rect rect, ManagerTab tab )
        {
            if ( tab == CurrentTab )
            {
                GUI.color = GenUI.MouseoverColor;
                GUI.DrawTexture( rect, tab.Icon );
                GUI.color = Color.white;
            }
            else if ( Widgets.ImageButton( rect, tab.Icon ) )
            {
                GoTo( tab );
            }
            TooltipHandler.TipRegion( rect, tab.Label );
        }

        public override void PostClose()
        {
            base.PostClose();
            CurrentTab.PostClose();
        }

        public override void PostOpen()
        {
            base.PostOpen();
            CurrentTab.PostOpen();
        }

        public override void PreClose()
        {
            base.PreClose();
            CurrentTab.PreClose();
        }

        public override void PreOpen()
        {
            base.PreOpen();
            CurrentTab.PreOpen();
        }
    }
}