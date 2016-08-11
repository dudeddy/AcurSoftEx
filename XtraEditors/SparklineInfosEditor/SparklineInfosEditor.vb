Imports System.ComponentModel
Imports AcurSoft.XtraGrid
Imports AcurSoft.XtraGrid.Views.Grid.Summary
Imports DevExpress.Data
Imports DevExpress.Sparkline

Namespace AcurSoft.XtraEditors.Controls


    Public Class SparklineInfosEditor
        Public Sub New()
            InitializeComponent()
            If Not Me.DesignMode Then
                Dim dic As New Dictionary(Of SparklineViewType, String)
                dic.Add(SparklineViewType.Area, SparklineViewType.Area.ToString())
                dic.Add(SparklineViewType.Bar, SparklineViewType.Bar.ToString())
                dic.Add(SparklineViewType.Line, SparklineViewType.Line.ToString())
                dic.Add(SparklineViewType.WinLoss, "Win / Loss")
                Me.edViewType.Properties.DataSource = dic
                Dim dic2 As New Dictionary(Of ColumnSortOrder, String)
                dic2.Add(ColumnSortOrder.None, "No order")
                dic2.Add(ColumnSortOrder.Ascending, "Ascending")
                dic2.Add(ColumnSortOrder.Descending, "Descending")
                Me.edOrderDirection.Properties.DataSource = dic2
            End If
        End Sub

        Public ReadOnly Property ViewType As SparklineViewType
            Get
                If Me.edViewType.EditValue Is Nothing Then Return SparklineViewType.Line
                Return DirectCast(Me.edViewType.EditValue, SparklineViewType)
            End Get
        End Property

        Public ReadOnly Property OrderDirection As ColumnSortOrder
            Get
                If Me.edOrderDirection.EditValue Is Nothing Then Return ColumnSortOrder.None
                Return DirectCast(Me.edOrderDirection.EditValue, ColumnSortOrder)
            End Get
        End Property

        Private _Infos As GridColumnSummaryItemExSparklineInfos
        <Browsable(False)>
        Public Property Infos As GridColumnSummaryItemExSparklineInfos
            Get
                If _Infos Is Nothing Then Return Nothing
                _Infos.ViewType = Me.ViewType
                _Infos.HighlightMaxPoint = Me.edHighlightMaxPoint.Checked
                _Infos.HighlightMinPoint = Me.edHighlightMinPoint.Checked
                _Infos.LineColor = Me.edColor.Color.ToArgb

                _Infos.MaxPointColor = Me.edMaxPointColor.Color.ToArgb
                _Infos.MinPointColor = Me.edMinPointColor.Color.ToArgb
                _Infos.HighlightNegativePoints = Me.edHighlightNegativePoints.Checked
                _Infos.UseExpression = Me.edUseExpression.Checked
                _Infos.Expression = Me.edExpression.Text
                _Infos.OrderDirection = Me.OrderDirection
                If Me.OrderDirection = ColumnSortOrder.None Then
                    _Infos.UseOrderExpression = False
                    _Infos.OrderExpression = ""
                Else
                    _Infos.UseOrderExpression = Me.edUseOrderExpression.Checked
                    _Infos.OrderExpression = Me.edOrderExpression.Text
                End If

                Return _Infos
            End Get
            Set(value As GridColumnSummaryItemExSparklineInfos)
                If value Is Nothing Then Return
                _Infos = value
                _IsSetting = True
                Me.edViewType.EditValue = _Infos.ViewType
                Me.edHighlightMaxPoint.Checked = _Infos.HighlightMaxPoint
                Me.edHighlightMinPoint.Checked = _Infos.HighlightMinPoint
                Me.edColor.Color = Color.FromArgb(_Infos.LineColor)
                Me.edMaxPointColor.Color = Color.FromArgb(_Infos.MaxPointColor)
                Me.edMinPointColor.Color = Color.FromArgb(_Infos.MinPointColor)
                Me.edHighlightNegativePoints.Checked = _Infos.HighlightNegativePoints
                Me.edSparkline.Properties.View = _Infos.CreateView
                Me.edSparkline.EditValue = _Infos.GetSparklineData()
                If _Infos.ViewType = SparklineViewType.WinLoss Then
                    Me.lcgWinLoss.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Else
                    Me.lcgWinLoss.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
                Me.edUseExpression.Checked = _Infos.UseExpression
                Me.edExpression.Text = _Infos.Expression
                Me.edOrderDirection.EditValue = _Infos.OrderDirection
                If _Infos.OrderDirection = ColumnSortOrder.None Then
                    Me.edUseOrderExpression.Checked = False
                    Me.edOrderExpression.Text = ""
                Else
                    Me.edUseOrderExpression.Checked = _Infos.UseOrderExpression
                    Me.edOrderExpression.Text = _Infos.OrderExpression
                End If

                _IsSetting = False
            End Set
        End Property

        Private _IsSetting As Boolean
        Private Sub edViewType_EditValueChanged(sender As Object, e As EventArgs) Handles edViewType.EditValueChanged, edOrderExpression.EditValueChanged, edMinPointColor.EditValueChanged, edMaxPointColor.EditValueChanged, edHighlightNegativePoints.EditValueChanged, edHighlightMinPoint.EditValueChanged, edHighlightMaxPoint.EditValueChanged, edColor.EditValueChanged, edExpression.EditValueChanged
            Me.UpdatePreview()
        End Sub

        Private Sub UpdatePreview()
            If _IsSetting Then Return
            _IsSetting = False

            If Me.ViewType = SparklineViewType.WinLoss Then
                Me.lcgWinLoss.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Else
                Me.lcgWinLoss.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            End If
            If _Infos Is Nothing Then Return
            Dim view As SparklineViewBase = SparklineViewBase.CreateView(Me.ViewType)

            Me.edSparkline.Properties.View = view
            With view
                .HighlightMaxPoint = Me.edHighlightMaxPoint.Checked
                .HighlightMinPoint = Me.edHighlightMinPoint.Checked
                .Color = Me.edColor.Color
                .MaxPointColor = Me.edMaxPointColor.Color
                .MinPointColor = Me.edMinPointColor.Color
                Select Case Me.ViewType
                    Case SparklineViewType.Line
                        With DirectCast(view, LineSparklineView)
                            .HighlightNegativePoints = Me.edHighlightNegativePoints.Checked
                        End With
                    Case SparklineViewType.Area
                        With DirectCast(view, AreaSparklineView)
                            .HighlightNegativePoints = Me.edHighlightNegativePoints.Checked
                        End With
                    Case SparklineViewType.Bar
                        With DirectCast(view, BarSparklineView)
                            .HighlightNegativePoints = Me.edHighlightNegativePoints.Checked
                        End With
                    Case SparklineViewType.WinLoss
                        With DirectCast(view, WinLossSparklineView)
                            '.HighlightNegativePoints = Me.HighlightNegativePoints
                        End With
                End Select

            End With
            Me.edSparkline.EditValue = _Infos.GetSparklineData(
              Me.edUseExpression.Checked,
              Me.edExpression.Text,
              Me.OrderDirection,
              Me.edUseOrderExpression.Checked,
              Me.edOrderExpression.Text)
            _IsSetting = False

        End Sub

        Private Sub edUseExpression_CheckedChanged(sender As Object, e As EventArgs) Handles edUseExpression.CheckedChanged, edUseExpression.EditValueChanged
            If Me.edUseExpression.Checked Then
                Me.lciExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Else
                Me.lciExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            End If
            Me.UpdatePreview()
        End Sub

        Private Sub edUseOrderExpression_CheckedChanged(sender As Object, e As EventArgs) Handles edUseOrderExpression.CheckedChanged, edUseOrderExpression.EditValueChanged
            If Me.edUseOrderExpression.Checked Then
                Me.lciOrderExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                'Me.lciOrderDirection.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Else
                Me.lciOrderExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                'Me.lciOrderDirection.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            End If
            Me.UpdatePreview()
        End Sub

        Private Sub edOrderDirection_EditValueChanged(sender As Object, e As EventArgs) Handles edOrderDirection.EditValueChanged
            If Me.OrderDirection = ColumnSortOrder.None Then
                Me.lciUseOrderExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Else
                Me.lciUseOrderExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            End If
            Me.UpdatePreview()
        End Sub

        Private Sub edPreview_CheckedChanged(sender As Object, e As EventArgs) Handles edPreview.CheckedChanged
            If Me.edPreview.Checked Then
                Me.lciPreview.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Else
                Me.lciPreview.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            End If

        End Sub
    End Class
End Namespace
