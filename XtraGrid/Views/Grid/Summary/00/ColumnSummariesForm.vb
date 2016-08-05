Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Linq
Imports AcurSoft.Data
Imports DevExpress.Data
Imports DevExpress.Data.Summary
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid

Namespace AcurSoft.XtraGrid.Views.Grid.Extenders


    Public Class ColumnSummariesForm
        Private _Data As BindingList(Of SummaryElement)
        'Private _SummaryTypeDic As Dictionary(Of SummaryItemTypeEx2, String)
        Public ReadOnly Property View As GridView
        Public ReadOnly Property GridColumn As GridColumn
        Private _Columns As DataTable
        Private _RedFoo As RepositoryItem

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()
            _RedFoo = New RepositoryItem
            ' Add any initialization after the InitializeComponent() call.
            _Data = New BindingList(Of SummaryElement)

            Me.redSummaryType.DataSource = CustomSummaryHelper.SummaryTypeDic

            Me.redSummaryType.ShowFooter = False
            Me.redSummaryType.ShowHeader = False
            Me.GridControl.DataSource = _Data
        End Sub

        'Public Sub New(view As GridView)
        Public Sub New(gridColumn As GridColumn)
            Me.New
            _GridColumn = gridColumn
            _View = DirectCast(gridColumn.View, GridView)
            _Columns = CustomSummaryHelper.GetGetSummaryFieldData(_View)
            Me.redColumn.DataSource = _Columns
            redColumn.DisplayMember = "caption"
            redColumn.ValueMember = "field"
            redColumn.PopulateColumns()
            redColumn.Columns.Item(0).Visible = False
            Me.redColumn.ShowFooter = False
            Me.redColumn.ShowHeader = False
            Dim index As Integer = 0
            For Each si As GridColumnSummaryItem In Me.GridColumn.Summary.OfType(Of GridColumnSummaryItem).OrderBy(Function(q) q.Index)
                'Dim summaryInfo As ColumnSummaryTagInfo = ColumnSummaryTagInfo.GetInstance(si)
                If TypeOf si Is GridColumnSummaryItemEx Then
                    Dim siEx As GridColumnSummaryItemEx = DirectCast(si, GridColumnSummaryItemEx)
                    _Data.Add(New SummaryElement With {.Col = si.FieldName, .DisplayFormat = si.DisplayFormat, .Info = siEx.Info, .SummaryType = siEx.SummaryTypeEx, .Order = index})

                Else
                    _Data.Add(New SummaryElement With {.Col = si.FieldName, .DisplayFormat = si.DisplayFormat, .Info = Nothing, .SummaryType = si.SummaryType.AsOf(Of SummaryItemTypeEx2), .Order = index})

                End If
                index += 1
            Next
        End Sub

        Private Sub btnUp_Click(sender As Object, e As System.EventArgs) Handles btnUp.Click
            If Me.GridView.GetFocusedRow Is Nothing Then Return
            Dim index As Integer = System.Convert.ToInt32(Me.GridView.GetFocusedRowCellValue("Order"))
            Dim prevIndex As Integer = index - 1
            If prevIndex = -1 Then Return
            Me.GridView.BeginDataUpdate()

            Dim prevElm As SummaryElement = _Data.FirstOrDefault(Function(q) q.Order = prevIndex)
            Dim Elm As SummaryElement = _Data.FirstOrDefault(Function(q) q.Order = index)

            prevElm.Order = index
            Elm.Order = prevIndex
            Me.GridView.EndDataUpdate()
        End Sub


        Private Sub btnDown_Click(sender As Object, e As System.EventArgs) Handles btnDown.Click
            If Me.GridView.GetFocusedRow Is Nothing Then Return
            Dim index As Integer = System.Convert.ToInt32(Me.GridView.GetFocusedRowCellValue("Order"))
            If index = _Data.Max(Function(q) q.Order) Then Return
            Me.GridView.BeginDataUpdate()
            Dim nextIndex As Integer = index + 1

            Dim nextElm As SummaryElement = _Data.FirstOrDefault(Function(q) q.Order = nextIndex)
            Dim Elm As SummaryElement = _Data.FirstOrDefault(Function(q) q.Order = index)

            nextElm.Order = index
            Elm.Order = nextIndex
            Me.GridView.EndDataUpdate()

        End Sub


        Private Sub GridView_CustomRowCellEditForEditing(sender As Object, e As DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs) Handles GridView.CustomRowCellEditForEditing
            'If Me.GridView.IsNewItemRow(e.RowHandle) Then
            If e.Column Is Nothing Then Return
            Select Case e.Column.FieldName
                Case "Col"
                    Dim red As New RepositoryItemLookUpEdit
                    red.Assign(Me.redColumn)

                    Dim dt As DataTable = _Columns.Clone
                    For i As Integer = 0 To _Columns.Rows.Count - 1
                        Dim r As DataRow = _Columns.Rows(i)
                        If _Data.Any(Function(q) q.Col = r.Item(0).ToString()) Then Continue For
                        Dim nr As DataRow = dt.NewRow
                        nr.ItemArray = _Columns.Rows(i).ItemArray
                        dt.Rows.Add(nr)
                    Next
                    dt.AcceptChanges()
                    red.DataSource = dt
                    e.RepositoryItem = red
                Case "Info"
                    Dim st As SummaryItemTypeEx2 = DirectCast(DirectCast(sender, GridView).GetRowCellValue(e.RowHandle, "SummaryType"), SummaryItemTypeEx2)
                    If SummaryItemTypeHelperEx.IsTopButtom(st) Then
                        e.RepositoryItem = Me.redTop
                    Else
                        e.RepositoryItem = _RedFoo
                    End If
                Case "DisplayFormat"
                    Dim st As SummaryItemTypeEx2 = DirectCast(DirectCast(sender, GridView).GetRowCellValue(e.RowHandle, "SummaryType"), SummaryItemTypeEx2)
                    If st = SummaryItemTypeEx2.None Then
                        e.RepositoryItem = _RedFoo
                    End If

                Case "SummaryType"
                    Dim field As String = DirectCast(sender, GridView).GetRowCellValue(e.RowHandle, "Col").ToString()
                    Dim col As GridColumn = Me.View.Columns(field)
                    Dim colType As Type = col.ColumnType
                    If Not CustomSummaryHelper.CanApplySumSummary(col) Then
                        Dim red As New RepositoryItemLookUpEdit
                        red.Assign(Me.redSummaryType)
                        red.DataSource = CustomSummaryHelper.GetSummaryTypeDic(col)
                        e.RepositoryItem = red
                    End If
            End Select
        End Sub


        Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click
            If _Data.Any(Function(q) String.IsNullOrEmpty(q.Col)) Then Return
            If _Data.Any(Function(q) q.SummaryType = SummaryItemTypeEx2.None) Then Return
            If _Data.Count = _Columns.Rows.Count Then Return
            Me.GridView.AddNewRow()
        End Sub

        Private Sub GridView_InitNewRow(sender As Object, e As InitNewRowEventArgs) Handles GridView.InitNewRow
            Me.GridView.SetRowCellValue(e.RowHandle, "Col", Me.GridColumn.FieldName)
            Me.GridView.SetRowCellValue(e.RowHandle, "Order", Me.GridView.RowCount - 1)
        End Sub


        Public Class SummaryElement
            Public Property Col As String
            'Public Property SortOn As DevExpress.XtraGrid.ColumnSortMode = DevExpress.XtraGrid.ColumnSortMode.Default
            'Public Property Direction As DevExpress.Data.ColumnSortOrder = DevExpress.Data.ColumnSortOrder.Descending
            Public Property Info As Object
            Public Property SummaryType As SummaryItemTypeEx2 = SummaryItemTypeEx2.None
            Public Property Order As Integer
            Public Property DisplayFormat As String = "{0}"

        End Class

        Private Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
            Me.SaveChanges()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        End Sub

        Public Sub SaveChanges()
            Me.GridColumn.Summary.BeginUpdate()
            Me.GridColumn.Summary.Clear()
            Dim x As Integer = 0
            For Each elm As SummaryElement In _Data.OrderBy(Function(q) q.Order)
                If String.IsNullOrEmpty(elm.Col) OrElse elm.SummaryType = SummaryItemTypeEx2.None Then Continue For
                Dim gsi As New GridColumnSummaryItemEx(Me.View, elm.SummaryType, elm.Col, elm.DisplayFormat, elm.Info)
                Me.GridColumn.Summary.Add(gsi)
                x += 1
            Next
            If x = 0 Then
                Me.GridColumn.Summary.Add(New GridColumnSummaryItemEx(SummaryItemType.None, Me.GridColumn.FieldName, "{0}"))
            Else
                Me.View.OptionsView.ShowFooter = True
            End If
            Me.GridColumn.Summary.EndUpdate()

        End Sub

        Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        End Sub

        Private Sub btnRemove_Click(sender As Object, e As System.EventArgs) Handles btnRemove.Click
            If Me.GridView.GetFocusedRow Is Nothing Then Return
            Dim index As Integer = System.Convert.ToInt32(Me.GridView.GetFocusedRowCellValue("Order"))
            Me.GridView.BeginDataUpdate()
            Dim elm As SummaryElement = _Data.FirstOrDefault(Function(q) q.Order = index)
            _Data.Remove(elm)
            Dim lst As List(Of SummaryElement) = _Data.OrderBy(Function(q) q.Order).ToList
            Dim x As Integer = 0
            For Each se As SummaryElement In lst
                se.Order = x
                x += 1
            Next
            Me.GridView.EndDataUpdate()

        End Sub

        Private Sub GridView_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView.CellValueChanged
            If e.Column Is Nothing Then Return
            Select Case e.Column.FieldName
                Case "Col"
                    'If e.Value Is Nothing Then Return
                    'Dim col As String = e.Value.ToString
                    With DirectCast(sender, GridView)
                        .SetRowCellValue(e.RowHandle, "DisplayFormat", "")
                        .SetRowCellValue(e.RowHandle, "SummaryType", SummaryItemTypeEx2.None)
                        .SetRowCellValue(e.RowHandle, "Info", Nothing)
                    End With

                Case "Info"
                    Dim st As SummaryItemTypeEx2 = DirectCast(DirectCast(sender, GridView).GetRowCellValue(e.RowHandle, "SummaryType"), SummaryItemTypeEx2)
                    Dim info As Object = e.Value
                    'If info Is Nothing Then
                    '    info = 5
                    'End If
                    Dim field As String = DirectCast(sender, GridView).GetRowCellValue(e.RowHandle, "Col").ToString
                    Dim col As GridColumn = Me.View.Columns(field)
                    Dim displayFormat As String = CustomSummaryHelper.GetSummaryTypeDisplayFormat(st, col, info)
                    DirectCast(sender, GridView).SetRowCellValue(e.RowHandle, "DisplayFormat", displayFormat)

                Case "SummaryType"
                    Dim st As SummaryItemTypeEx2 = DirectCast(e.Value, SummaryItemTypeEx2)
                    Dim field As String = DirectCast(sender, GridView).GetRowCellValue(e.RowHandle, "Col").ToString
                    Dim col As GridColumn = Me.View.Columns(field)
                    Dim info As Object = GridColumnSummaryItemEx.FixSummaryInfo(st)
                    Dim gv As GridView = DirectCast(sender, GridView)
                    gv.SetRowCellValue(e.RowHandle, "Info", info)
                    Dim displayFormat As String = CustomSummaryHelper.GetSummaryTypeDisplayFormat(st, col, info)
                    gv.SetRowCellValue(e.RowHandle, "DisplayFormat", displayFormat)

            End Select
        End Sub
    End Class
End Namespace
