﻿using System;
using Markdig;
using UnityEditor;
using UnityEngine;
using Markdig.Extensions.JiraLinks;
using Markdig.Extensions.Tables;
using Sirenix.Utilities;

namespace MG.MDV
{
    public class MarkdownViewer
    {
        public static readonly Vector2 Margin = new Vector2(6.0f, 4.0f);

        private GUISkin mSkin = null;
        private string mText = string.Empty;
        private string mCurrentPath = string.Empty;
        private HandlerImages mHandlerImages = new HandlerImages();
        private HandlerNavigate mHandlerNavigate = new HandlerNavigate();

        private Layout mLayout = null;
        private bool mRaw = false;

        private static History mMDHistory = new History();


        public string MarkdownFilePath { get; set; }

        public MarkdownViewer(GUISkin skin, string path, string content)
        {
            mSkin = skin;
            mCurrentPath = path;
            mText = content;

            mMDHistory.OnOpen(mCurrentPath);
            mLayout = ParseDocument();

            mHandlerImages.CurrentPath = mCurrentPath;

            mHandlerNavigate.CurrentPath = mCurrentPath;
            mHandlerNavigate.History = mMDHistory;
            mHandlerNavigate.FindBlock = (id) => mLayout.Find(id);
            mHandlerNavigate.ScrollTo = (pos) => { }; // TODO: mScrollPos.y = pos;
        }

        //------------------------------------------------------------------------------

        public bool Update()
        {
            return mHandlerImages.Update();
        }


        //------------------------------------------------------------------------------

        Layout ParseDocument()
        {
            var context = new Context(mSkin, mHandlerImages, mHandlerNavigate);
            var builder = new LayoutBuilder(context);
            var renderer = new RendererMarkdown(builder);

            var pipelineBuilder = new MarkdownPipelineBuilder()
                .UseAutoLinks();

            if (!string.IsNullOrEmpty(Preferences.JIRA))
            {
                pipelineBuilder.UseJiraLinks(new JiraLinkOptions(Preferences.JIRA));
            }


            if (Preferences.PipedTables)
            {
                pipelineBuilder.UsePipeTables(new PipeTableOptions
                {
                    RequireHeaderSeparator = Preferences.PipedTablesRequireRequireHeaderSeparator
                });
            }


            var pipeline = pipelineBuilder.Build();
            pipeline.Setup(renderer);

            var doc = Markdig.Markdown.Parse(mText, pipeline);
            renderer.Render(doc);

            return builder.GetLayout();
        }


        //------------------------------------------------------------------------------

        private void ClearBackground(float height)
        {
            var rectFullScreen = new Rect(0.0f, 0.0f, Screen.width, Mathf.Max(height, Screen.height));
            GUI.DrawTexture(rectFullScreen, mSkin.window.normal.background, ScaleMode.StretchToFill, false);
        }


        private Vector2 mScrollPos;
        //------------------------------------------------------------------------------

        public void Draw()
        {
            GUI.skin = mSkin;
            GUI.enabled = true;

            // useable width of inspector windows

            var contentWidth = EditorGUIUtility.currentViewWidth - mSkin.verticalScrollbar.fixedWidth - 2.0f * Margin.x;


            // draw content

            if (mRaw)
            {
                var style = mSkin.GetStyle("raw");
                var width = contentWidth - mSkin.button.fixedHeight;
                var height = style.CalcHeight(new GUIContent(mText), width);

                ClearBackground(height);
                EditorGUILayout.SelectableLabel(mText, style,
                    new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) });
            }
            else
            {
                ClearBackground(mLayout.Height);
                DrawMarkdown(contentWidth);
            }

            DrawToolbar(contentWidth);
        }


        public void Draw(Rect rect)
        {
            GUI.skin = mSkin;
            GUI.enabled = true;

            // useable width of inspector windows

            var contentWidth = EditorGUIUtility.currentViewWidth - mSkin.verticalScrollbar.fixedWidth - 2.0f * Margin.x;


            // draw content

            if (mRaw)
            {
                var style = mSkin.GetStyle("raw");
                var width = contentWidth - mSkin.button.fixedHeight;
                var height = style.CalcHeight(new GUIContent(mText), width);

                ClearBackground(height);
                EditorGUILayout.SelectableLabel(mText, style,
                    new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) });
            }
            else
            {
                ClearBackground(mLayout.Height);
                DrawMarkdown(rect.width);
            }

            DrawToolbar(contentWidth);
        }
        
        public void Draw(float testwidth)
        {
            GUI.skin = mSkin;
            GUI.enabled = true;

            // useable width of inspector windows

            var contentWidth = EditorGUIUtility.currentViewWidth - mSkin.verticalScrollbar.fixedWidth - 2.0f * Margin.x;


            // draw content

            if (mRaw)
            {
                var style = mSkin.GetStyle("raw");
                var width = contentWidth - mSkin.button.fixedHeight;
                var height = style.CalcHeight(new GUIContent(mText), width);

                ClearBackground(height);
                EditorGUILayout.SelectableLabel(mText, style,
                    new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) });
            }
            else
            {
                ClearBackground(mLayout.Height);
                DrawMarkdown(testwidth);
            }

            DrawToolbar(contentWidth);
        }


        void DrawRaw(Rect rect)
        {
            EditorGUI.SelectableLabel(rect, mText, GUI.skin.GetStyle("raw"));
        }


        //------------------------------------------------------------------------------

        void DrawToolbar(float contentWidth)
        {
            var style = GUI.skin.button;
            var size = style.fixedHeight;
            var btn = new Rect(Margin.x + contentWidth - size, Margin.y, size, size);

            if (GUI.Button(btn, string.Empty, GUI.skin.GetStyle(mRaw ? "btnRaw" : "btnFile")))
            {
                mRaw = !mRaw;
            }

            if (mRaw == false)
            {
                if (mMDHistory.CanForward)
                {
                    btn.x -= size;

                    if (GUI.Button(btn, string.Empty, GUI.skin.GetStyle("btnForward")))
                    {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>(mMDHistory.Forward());
                    }
                }

                if (mMDHistory.CanBack)
                {
                    btn.x -= size;

                    if (GUI.Button(btn, string.Empty, GUI.skin.GetStyle("btnBack")))
                    {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>(mMDHistory.Back());
                    }
                }
            }
        }

        //------------------------------------------------------------------------------

        void DrawMarkdown(float width)
        {
            switch (Event.current.type)
            {
                case EventType.Ignore:
                    break;

                case EventType.ContextClick:
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("View Source"), false, () => mRaw = !mRaw);

                    if (MarkdownFilePath.IsNullOrWhitespace())
                    {
                        menu.AddItem(new GUIContent("Select File"), false,
                            () =>
                            {
                                Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>(MarkdownFilePath);
                            });
                    }

                    menu.ShowAsContext();
                    break;

                case EventType.Layout:
                    GUILayout.Space(mLayout.Height);
                    mLayout.Arrange(width);
                    break;

                default:
                    mLayout.Draw();
                    break;
            }
        }


        public void UpdateText(string value)
        {
            if (mText != value)
            {
                mText = value;
                mLayout = ParseDocument();
            }
        }

        float ContentHeight(float width)
        {
            return mRaw ? GUI.skin.GetStyle("raw").CalcHeight(new GUIContent(mText), width) : mLayout.Height;
        }

        public void ResetScrollPos()
        {
            mScrollPos = Vector2.zero;
        }

        public void DrawWithRect(Rect rect)
        {
            GUI.skin = mSkin;
            GUI.enabled = true;

            // content rect

            var rectContainer = rect;


            // clear background

            var rectFullScreen = new Rect(0.0f, rectContainer.yMin - 4.0f, Screen.width, Screen.height);
            GUI.DrawTexture(rectFullScreen, mSkin.window.normal.background, ScaleMode.StretchToFill, false);

            // scroll window

            var padLeft = 8.0f;
            var padRight = 4.0f;
            var padHoriz = padLeft + padRight;
            var scrollWidth = GUI.skin.verticalScrollbar.fixedWidth;
            var minWidth = rectContainer.width - scrollWidth - padHoriz;
            var maxHeight = ContentHeight(minWidth);

            var hasScrollbar = maxHeight >= rectContainer.height;
            var contentWidth = hasScrollbar ? minWidth : rectContainer.width - padHoriz;
            var rectContent = new Rect(-padLeft, 0.0f, contentWidth, maxHeight);

            // draw content

            using (var scroll = new GUI.ScrollViewScope(rectContainer, mScrollPos, rectContent))
            {
                GUILayout.BeginHorizontal();

                mScrollPos = scroll.scrollPosition;

                if (mRaw)
                {
                    rectContent.width = minWidth - GUI.skin.button.fixedWidth;
                    DrawRaw(rectContent);
                }
                else
                {
                    DrawMarkdown(rectContainer.width);
                }

                GUILayout.Space(20); // scroll bar 增加 20 个像素
                GUILayout.EndHorizontal();
            }

            var style = GUI.skin.button;
            var size = style.fixedHeight;
            var btn = new Rect(Margin.x + contentWidth - size + 15, Margin.y + 30, size, size);

            if (GUI.Button(btn, string.Empty, GUI.skin.GetStyle(mRaw ? "btnRaw" : "btnFile")))
            {
                mRaw = !mRaw;
            }
        }
    }
}