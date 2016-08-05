Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Scrolling
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace AcurSoft.XtraGrid.Views.Grid.ViewInfo
    Public Class GridViewExInfo
        Inherits GridViewInfo

        Public Shadows ReadOnly Property View() As GridViewEx
            Get
                Return TryCast(MyBase.View, GridViewEx)
            End Get
        End Property
        'Public ReadOnly Property CustomPanelPainter() As ObjectPainter
        '	Get
        '		Return Painter.ElementsPainter.FilterPanel
        '	End Get
        'End Property
        Public Sub New(ByVal gridView As GridViewEx)
            MyBase.New(gridView)
        End Sub
#Region "Extra Panel"

        Protected ReadOnly Property XtraPanelBounds() As Rectangle
        Public ReadOnly Property ScrollInfo() As ScrollInfo
            Get
                'Return TryCast(View.GetType().GetProperty("ScrollInfo").GetValue(View, Nothing), ScrollInfo)
                Return Me.View.ScrollInfoEx
            End Get
        End Property
        Public Overrides Sub CalcRects(ByVal bounds As Rectangle, ByVal partital As Boolean)
            Dim r As Rectangle = Rectangle.Empty
            ViewRects.Bounds = bounds
            ViewRects.Scroll = CalcScrollRect()
            ViewRects.Client = CalcClientRect()
            FilterPanel.Bounds = Rectangle.Empty

            If Not partital Then
                CalcRectsConstants()
            End If

            If View.OptionsView.ShowIndicator Then
                ViewRects.IndicatorWidth = Math.Max(ScaleHorizontal(View.IndicatorWidth), ViewRects.MinIndicatorWidth)
            End If
            Dim minTop As Integer = ViewRects.Client.Top
            Dim maxBottom As Integer = ViewRects.Client.Bottom
            If View.OptionsView.ShowViewCaption Then
                r = ViewRects.Scroll
                r.Y = minTop
                r.Height = CalcViewCaptionHeight(ViewRects.Client)
                ViewRects.ViewCaption = r
                minTop = ViewRects.ViewCaption.Bottom
            End If
            minTop = UpdateFindControlVisibility(New Rectangle(ViewRects.Scroll.X, minTop, ViewRects.Scroll.Width, maxBottom - minTop), False).Y
            'minTop = UpdateCustomControlVisibility(New Rectangle(ViewRects.Scroll.X, minTop, ViewRects.Scroll.Width, maxBottom - minTop), False).Y
            minTop = UpdateXtraPanelControlVisibility(New Rectangle(ViewRects.Scroll.X, minTop, ViewRects.Scroll.Width, maxBottom - minTop)).Y
            If View.OptionsView.ShowGroupPanel Then
                r = ViewRects.Scroll
                r.Y = minTop
                r.Height = CalcGroupPanelHeight()
                ViewRects.GroupPanel = r
                minTop = ViewRects.GroupPanel.Bottom
            End If



            minTop = CalcRectsColumnPanel(minTop)
            ViewRects.VScrollLocation = minTop

            If View.IsShowFilterPanel Then
                r = ViewRects.Scroll
                Dim fPanel As Integer = GetFilterPanelHeight()
                r.Y = maxBottom - fPanel
                r.Height = fPanel
                FilterPanel.Bounds = r
                maxBottom = r.Top
            End If
            ViewRects.HScrollLocation = maxBottom
            If HScrollBarPresence = ScrollBarPresence.Visible Then
                If Not ScrollInfo.IsOverlapHScrollBar Then
                    maxBottom -= HScrollSize
                End If
            End If

            If View.OptionsView.ShowFooter Then
                r = ViewRects.Scroll
                r.Height = GetFooterPanelHeight()
                r.Y = maxBottom - r.Height
                ViewRects.Footer = r
                maxBottom = r.Top
            End If
            r = ViewRects.Client
            r.Y = minTop
            r.Height = maxBottom - minTop
            ViewRects.Rows = r

        End Sub

        Protected Function UpdateXtraPanelControlVisibility(ByVal client As Rectangle) As Rectangle
            If Not Me.View.OptionsView.ShowXtraPanel Then Return client
            If Me.View.XtraPanel Is Nothing Then Return client
            Dim bounds As Rectangle = client
            bounds.Height = View.XtraPanel.Height + View.SplitterControl.Size.Height
            _XtraPanelBounds = bounds
            bounds.Y = bounds.Bottom
            bounds.Height = (client.Bottom - bounds.Y)
            client = bounds
            Return client
        End Function
#End Region
    End Class
End Namespace
