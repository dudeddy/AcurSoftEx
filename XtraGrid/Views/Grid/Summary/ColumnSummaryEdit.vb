Imports AcurSoft.Data
Imports DevExpress.Data
Imports DevExpress.Data.Summary
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid

Namespace AcurSoft.XtraGrid.Views.Grid.Extenders


    Public Class ColumnSummaryConfig

        Private _Column As GridColumn
        Private _View As GridView
        'Private _SummaryInfo As ColumnSummaryTagInfo
        Private _Columns As DataTable

        Public Sub New()
            InitializeComponent()
            '_DoReplace = True
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

            'Me.edType.EditValue = _SummaryInfo.SummaryType
            If SummaryItemTypeHelperEx.IsTopButtom(si.SummaryTypeEx) Then
                Me.edTop.Value = Convert.ToDecimal(si.Info)
                Dim caption As String = CustomSummaryHelper.GetSummaryTypeCaption(si.SummaryTypeEx, col).Replace("50", "X").Replace("5", "X")
                Me.lciTop.Text = caption & " :"
                Me.lciTop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
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
                    If SummaryItemTypeHelperEx.IsTopButtom(st) Then
                        Me.edTop.Value = Convert.ToDecimal(Me.edTop.EditValue)
                        Dim caption As String = CustomSummaryHelper.GetSummaryTypeCaption(st, c).Replace("50", "X").Replace("5", "X")
                        Me.lciTop.Text = caption & " :"
                        Me.lciTop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                    Else
                        Me.lciTop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                    End If
                End Sub
            AddHandler Me.edFieldName.EditValueChanged, AddressOf edFieldName_EditValueChanged
            '_DoReplace = si.Collection.Count = 1
        End Sub

        Public Sub SaveChanges()
            Dim field As String = Me.edFieldName.EditValue.ToString()
            Dim st As SummaryItemTypeEx2 = DirectCast(Me.edType.EditValue, SummaryItemTypeEx2)
            _View.BeginDataUpdate()
            _Column.Summary.BeginUpdate()
            If st = SummaryItemTypeEx2.None AndAlso _GridColumnSummaryItemEx IsNot Nothing AndAlso _GridColumnSummaryItemEx.Collection.Count > 1 Then
                _GridColumnSummaryItemEx.Collection.Remove(_GridColumnSummaryItemEx)
            Else
                _GridColumnSummaryItemEx.ReAssign(field, st, Me.edDisplayFormat.Text, Me.edTop.EditValue)
                If st <> SummaryItemTypeEx2.None Then
                    _View.OptionsView.ShowFooter = True
                End If
            End If
            _Column.Summary.EndUpdate()
            _View.EndDataUpdate()

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
    End Class
End Namespace
