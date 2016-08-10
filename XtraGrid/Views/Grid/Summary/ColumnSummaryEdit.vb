Imports AcurSoft.Data
Imports DevExpress.Data
Imports DevExpress.Data.Summary
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid

Namespace AcurSoft.XtraGrid.Views.Grid.Extenders


    Public Class ColumnSummaryConfig

        Private _Column As GridColumn
        Private _View As GridView
        Private _Columns As DataTable

        Public Sub New()
            InitializeComponent()
        End Sub

        'Public Property DoReplace As Boolean

        'Private _GridColumnSummaryItem As GridColumnSummaryItem
        Private _GridColumnSummaryItemEx As GridColumnSummaryItemEx
        Public Sub New(col As GridColumn, si As GridColumnSummaryItemEx)
            Me.New()

            _Column = col
            _View = DirectCast(col.View, GridView)
            _Columns = CustomSummaryHelper.GetGetSummaryFieldData(_View)
            _GridColumnSummaryItemEx = si

            With Me.edFieldName.Properties
                .DataSource = _Columns
                .DisplayMember = "caption"
                .ValueMember = "field"
                .ShowFooter = False
                .ShowHeader = False
            End With
            Me.edFieldName.EditValue = col.FieldName

            Me.edType.Properties.DataSource = CustomSummaryHelper.SummaryTypeDic
            'Dim si As GridSummaryItem = col.SummaryItem
            '_SummaryInfo = New ColumnSummaryTagInfo(col.SummaryItem)

            Me.edFieldName.EditValue = si.FieldName
            Me.edType.EditValue = si.SummaryTypeEx

            'If si.SummaryType <> SummaryItemType.Custom Then
            '    Me.edType.EditValue = si.SummaryType.AsOf(Of SummaryItemTypeEx2)
            'ElseIf TypeOf si.Tag Is ColumnSummaryTagInfo Then
            '    Dim tag As ColumnSummaryTagInfo = DirectCast(si.Tag, ColumnSummaryTagInfo)
            '    Me.edType.EditValue = tag.SummaryType
            'End If
            Me.edDisplayFormat.Text = si.DisplayFormat
            If si.SummaryTypeEx = SummaryItemTypeEx2.Expression Then
                Me.edExpression.EditValue = si.Info

                Me.lcgExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

            ElseIf SummaryItemTypeHelperEx.IsTopButtom(si.SummaryTypeEx) Then
                Me.edTop.Value = Convert.ToDecimal(si.Info)
                Dim caption As String = CustomSummaryHelper.GetSummaryTypeCaption(si.SummaryTypeEx, col).Replace("50", "X").Replace("5", "X")
                Me.lciTop.Text = caption & " :"
                Me.lciTop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            ElseIf si.SummaryTypeEx = SummaryItemTypeEx2.Sparkline
                Me.SparklineInfosEditor1.Infos = DirectCast(_GridColumnSummaryItemEx.Info, GridColumnSummaryItemExSparklineInfos)
                Me.lcgDisplayFormat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lciSparkline.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            End If

            Me.edDisplayFormat.EditValue = col.SummaryItem.DisplayFormat
            AddHandler Me.edType.EditValueChanged,
                Sub(s, a)
                    If Me.edType.EditValue Is Nothing Then Return
                    If Me.edFieldName.EditValue Is Nothing Then Return

                    Dim st As SummaryItemTypeEx2 = DirectCast(Me.edType.EditValue, SummaryItemTypeEx2)
                    Dim c As GridColumn = _View.Columns(Me.edFieldName.EditValue.ToString())
                    If c Is Nothing Then Return
                    Dim displayFormat As String = CustomSummaryHelper.GetSummaryTypeDisplayFormat(st, c, Me.edTop.EditValue)
                    Me.edDisplayFormat.Text = displayFormat
                    If st = SummaryItemTypeEx2.Expression Then
                        Me.edExpression.Text = "Sum(" & c.FieldName & ")"
                        Me.lciTop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.lcgExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                        Me.lciSparkline.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.lcgDisplayFormat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                    ElseIf SummaryItemTypeHelperEx.IsTopButtom(st) Then
                        Me.edTop.Value = Convert.ToDecimal(Me.edTop.EditValue)
                        Dim caption As String = CustomSummaryHelper.GetSummaryTypeCaption(st, c).Replace("50", "X").Replace("5", "X")
                        Me.lciTop.Text = caption & " :"
                        Me.lciTop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                        Me.lcgExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.lciSparkline.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.lcgDisplayFormat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                    ElseIf st = SummaryItemTypeEx2.Sparkline
                        Me.SparklineInfosEditor1.Infos = DirectCast(_GridColumnSummaryItemEx.Info, GridColumnSummaryItemExSparklineInfos)
                        Me.lciTop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.lcgExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.lciSparkline.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                        Me.lcgDisplayFormat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                    Else
                        Me.lciTop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.lcgExpression.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.lciSparkline.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.lcgDisplayFormat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

                    End If
                    'Me.GetHelp()
                End Sub
            AddHandler Me.edFieldName.EditValueChanged, AddressOf edFieldName_EditValueChanged
            Me.GetHelp()
        End Sub

        Public Sub GetHelp()
            Dim info As String = "<b><color=blue>Sum :</color></b> Sum(expression) or [this][condition].Sum(expression)<br>"
            info &= "<b><color=blue>Avg :</color></b> Avg(expression) or [this][condition].Avg(expression)<br>"
            info &= "<b><color=blue>Count :</color></b> Count(expression) or [this][condition].Count()<br>"
            info &= "<b><color=blue>SumTop :</color></b> SumTop(expression, top, <u>is percent</u>, <u>time precision 's','m','h','d'</u>)<br>"
            info &= "*- time precision apply for time expression : 'd'(days), 'h'(hours), 'm'(mins), 's'(secs).<br>"
            info &= "*- eg1: SumTop([col1], 10)- eg2: SumTop([col1], 30, true) <br>"
            info &= "*- eg3: SumTop([col1], 10, 'h')- eg2: SumTop([col1], 30, true, 'h') : sum hours only<br>"
            info &= "<b><color=blue>SumBottom :</color></b> semilar to ""SumTop""<br>"
            info &= "<b><color=blue>AvgTop :</color></b> semilar to ""SumTop""<br>"
            info &= "<b><color=blue>AvgBottom :</color></b> semilar to ""SumTop""<br>"
            info &= "<b><color=blue>Rank :</color></b> semilar to ""SumTop""<br>"
            info &= "Rank(expression, rank, <u>is percent</u>, <u>time precision 's'/'m'/'h'/'d'</u>)<br>"
            info &= "*- if rank is negative => (From bottom)<br>"
            info &= "<b><color=blue>DistinctCount :</color></b> DistinctCount(expression, <u>time precision 's','m','h','d'</u>)<br>"
            info &= "<b><color=blue>DistinctSum :</color></b> DistinctSum(expression, <u>top / bottom</u>, <u>is percent</u>, <u>time precision 's','m','h','d'</u>)<br>"
            info &= "*- if top / bottom is negative => (From bottom)<br>"
            info &= "*- eg1: DistinctSum([col1]) => Sum of unique values of ""col1"" <br>"
            info &= "*- eg2: DistinctSum([col1], -5) => Sum of bottom 5 unique values of ""col1"" <br>"
            info &= "*- eg3: DistinctSum([col1], 30, true) => Sum of top 30% unique values of ""col1"" <br>"
            info &= "*- eg4: DistinctSum([col1], 'm') => Sum of unique values of ""col1"" : ""col1"" must be timespan, 'm' means seconds will be ignored.<br>"
            info &= "<b><color=blue>DistinctAvg :</color></b> semilar to ""DistinctSum""<br>"
            info &= "<b><color=blue>ValueAt :</color></b> ValueAt(expression, index, <u>time precision 's','m','h','d'</u>)<br>"

            Me.btn_help_expression.SuperTip = New SuperToolTip()
            Me.btn_help_expression.SuperTip.Items.Add(info).AllowHtmlText = DefaultBoolean.True

            info = "<b><color=blue>For numeric values</color></b><br>"
            info &= "*- <b>'c'</b> currency amount => eg 10.5 :: '{0:c}' : $ 10.50, '{0:c3}' : $ 10.500<br>"
            info &= "*- <b>'n'</b> amount => eg 1.5 :: '{0:n}' : 10.50, '{0:n3}' : 10.500<br>"
            info &= "*- <b>'0'</b> The digit is always displayed, '#' The digit is displayed only when needed<br>"
            info &= "*- eg: 10.5 :: '{0:#.00}' : 10.50,'{0:#.000}' : 10.500, '{0:#.#}' : 10.5<br>"
            info &= "*- <b>'{0:positive;negative;zero}'</b><br>"
            info &= "*- eg: '{0:#.00;[#.0];Zero}' :: 10.5 => '10.50', -20.5 => '[20.50]', 0 => 'Zero'<br><br>"
            info &= "<b><color=blue>For timespan</color></b><br>"
            info &= "*- days: <b>'d' or '%d'</b> without leading zero, 'dd' without leading zero<br>"
            info &= "*- hours: <b>'h' or '%h'</b> without leading zero, 'hh' without leading zero<br>"
            info &= "*- minutes: <b>'m' or '%m'</b> without leading zero, 'mm' without leading zero<br>"
            info &= "*- seconds: <b>'s' or '%s'</b> without leading zero, 'ss' without leading zero<br>"
            info &= "*- any character: <b>'\character'</b> => eg: '\.', '\:' <br>"
            info &= "*- eg: '{0:d\.hh\:mm\:ss}', '{0:d\.hh\:mm}' <br>"

            Me.btn_help_display_format.SuperTip = New SuperToolTip()
            Me.btn_help_display_format.SuperTip.Items.Add(info).AllowHtmlText = DefaultBoolean.True

        End Sub

        Public Sub SaveChanges()
            Dim field As String = Me.edFieldName.EditValue.ToString()
            Dim st As SummaryItemTypeEx2 = DirectCast(Me.edType.EditValue, SummaryItemTypeEx2)
            _View.BeginDataUpdate()
            _Column.Summary.BeginUpdate()
            If st = SummaryItemTypeEx2.None AndAlso _GridColumnSummaryItemEx IsNot Nothing AndAlso _GridColumnSummaryItemEx.Collection.Count > 1 Then
                _GridColumnSummaryItemEx.Collection.Remove(_GridColumnSummaryItemEx)
            Else
                If st = SummaryItemTypeEx2.Expression Then
                    _GridColumnSummaryItemEx.ReAssign(field, st, Me.edDisplayFormat.Text, Me.edExpression.EditValue)
                ElseIf st = SummaryItemTypeEx2.Sparkline
                    Dim info As GridColumnSummaryItemExSparklineInfos = Me.SparklineInfosEditor1.Infos
                    'Me.SparklineInfosEditor1.edSparkline.EditValue
                    _GridColumnSummaryItemEx.ReAssign(field, st, "", Me.SparklineInfosEditor1.Infos)
                Else
                    _GridColumnSummaryItemEx.ReAssign(field, st, Me.edDisplayFormat.Text, Me.edTop.EditValue)
                End If
                If st <> SummaryItemTypeEx2.None Then
                    _View.OptionsView.ShowFooter = True
                End If
            End If
            _Column.Summary.EndUpdate()
            _View.EndDataUpdate()
            '_View.InvalidateFooter()
        End Sub

        Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
            Me.DialogResult = DialogResult.OK
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
        End Sub

        Private Sub edTop_EditValueChanged(sender As Object, e As EventArgs) Handles edTop.EditValueChanged
            'If _SummaryInfo Is Nothing Then Return
            If Me.edType.EditValue Is Nothing Then Return
            If Me.edFieldName.EditValue Is Nothing Then Return
            Dim st As SummaryItemTypeEx2 = DirectCast(Me.edType.EditValue, SummaryItemTypeEx2)
            Dim col As GridColumn = _View.Columns(Me.edFieldName.EditValue.ToString())
            If col Is Nothing Then Return
            If SummaryItemTypeHelperEx.IsTopButtom(st) Then
                Me.edDisplayFormat.Text = CustomSummaryHelper.GetSummaryTypeDisplayFormat(st, col, Me.edTop.Value)
            End If
        End Sub

        Private Sub edFieldName_EditValueChanged(sender As Object, e As EventArgs)
            If Me.edFieldName.EditValue Is Nothing Then Return
            Me.edType.Properties.DataSource = CustomSummaryHelper.GetSummaryTypeDic(_View.Columns(Me.edFieldName.EditValue.ToString()))
            Me.edType.EditValue = SummaryItemTypeEx2.None
        End Sub

        Private Sub btn_help_expression_Click(sender As Object, e As EventArgs) Handles btn_help_expression.Click, btn_help_display_format.Click
            Dim btn As SimpleButton = DirectCast(sender, SimpleButton)

            Dim tti As New DevExpress.Utils.ToolTipControlInfo(DirectCast(e, MouseEventArgs).Location, "") With {
                .AllowHtmlText = DevExpress.Utils.DefaultBoolean.True,
                .SuperTip = btn.SuperTip}

            If btn.ToolTipController Is Nothing Then
                btn.ToolTipController = New ToolTipController()
            End If
            btn.ToolTipController.ShowHint(tti)
        End Sub
    End Class
End Namespace
