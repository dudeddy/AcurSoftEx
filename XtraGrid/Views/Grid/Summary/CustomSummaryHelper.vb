Imports AcurSoft.Data
Imports AcurSoft.Data.Filtering
Imports AcurSoft.Data.Filtering.Summary
Imports AcurSoft.XtraGrid.Views.Grid.Extenders
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Summary
Imports DevExpress.Data.Summary
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
'Imports DevExpress.Data.Summary

Namespace AcurSoft.XtraGrid.Views.Grid.Summary

    Public Class CustomSummaryHelper
#Region "Commun Data"

        Private Shared _SummaryTypeDic As Dictionary(Of SummaryItemTypeEx2, String)
        Public Shared ReadOnly Property SummaryTypeDic As Dictionary(Of SummaryItemTypeEx2, String)
            Get
                If _SummaryTypeDic Is Nothing Then
                    _SummaryTypeDic = New Dictionary(Of SummaryItemTypeEx2, String)
                    _SummaryTypeDic.Add(SummaryItemType.None.AsOf(Of SummaryItemTypeEx2), "None")
                    _SummaryTypeDic.Add(SummaryItemType.Min.AsOf(Of SummaryItemTypeEx2), "Min")
                    _SummaryTypeDic.Add(SummaryItemType.Max.AsOf(Of SummaryItemTypeEx2), "Max")
                    _SummaryTypeDic.Add(SummaryItemType.Count.AsOf(Of SummaryItemTypeEx2), "Count")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.UniqueValuesCount, "Unique Values Count")
                    _SummaryTypeDic.Add(SummaryItemType.Sum.AsOf(Of SummaryItemTypeEx2), "Sum")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.TopXSum, "Top X Sum")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.TopXPercentSum, "Top X Percent Sum")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.BottomXSum, "Bottom X Sum")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.BottomXPercentSum, "Bottom X Percent Sum")
                    _SummaryTypeDic.Add(SummaryItemType.Average.AsOf(Of SummaryItemTypeEx2), "Average")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.TopXAvg, "Top X Avg")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.TopXPercentAvg, "Top X Percent Avg")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.BottomXAvg, "Bottom X Avg")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.BottomXPercentAvg, "Bottom X Percent Avg")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.Expression, "Expression")
                    _SummaryTypeDic.Add(SummaryItemTypeEx2.Sparkline, "Sparkline")

                End If
                Return _SummaryTypeDic
            End Get
        End Property

        Public Shared Function GetSummaryTypeDic(col As GridColumn) As Dictionary(Of SummaryItemTypeEx2, String)
            Dim colType As Type = col.ColumnType
            Dim canSum As Boolean = colType Is GetType(TimeSpan) OrElse SummaryItemTypeHelper.CanApplySummary(SummaryItemType.Sum, colType)
            Dim canAvg As Boolean = colType Is GetType(TimeSpan) OrElse SummaryItemTypeHelper.CanApplySummary(SummaryItemType.Average, colType)
            If Not canAvg OrElse Not canSum Then
                Dim dic As Dictionary(Of SummaryItemTypeEx2, String) = CustomSummaryHelper.SummaryTypeDic.Where(
                            Function(q)
                                Dim b As Boolean = True
                                If Not canSum Then
                                    b = Not {SummaryItemTypeEx2.Sum, SummaryItemTypeEx2.TopXSum, SummaryItemTypeEx2.BottomXSum}.Contains(q.Key)
                                End If
                                If b AndAlso Not canAvg Then
                                    b = Not {SummaryItemTypeEx2.Average, SummaryItemTypeEx2.TopXAvg, SummaryItemTypeEx2.BottomXAvg}.Contains(q.Key)
                                End If
                                Return b
                            End Function).ToDictionary(Of SummaryItemTypeEx2, String)(Function(q) q.Key, Function(q) q.Value)
                Return dic
            Else
                Return CustomSummaryHelper.SummaryTypeDic
            End If
        End Function

        Public Shared Function GetGetSummaryFieldData(view As GridView) As DataTable
            Dim dt As New DataTable
            dt.Columns.Add("field")
            dt.Columns.Add("caption")
            For Each c As GridColumn In view.Columns.OfType(Of GridColumn)
                Dim nr As DataRow = dt.NewRow
                nr.SetField(Of String)("field", c.FieldName)
                nr.SetField(Of String)("caption", c.GetCaption())
                dt.Rows.Add(nr)
            Next
            dt.AcceptChanges()
            Return dt
        End Function
#End Region

#Region "Calculate"
#Region "Unique Count"

        Public Shared Sub GetUniqueValuesCount(ByVal view As GridView, ByVal e As CustomSummaryEventArgs)
            Dim summaryItem As GridColumnSummaryItemEx = TryCast(e.Item, GridColumnSummaryItemEx)
            If Not e.IsTotalSummary OrElse summaryItem.SummaryTypeEx <> SummaryItemTypeEx2.UniqueValuesCount Then Return
            Select Case e.SummaryProcess
                Case DevExpress.Data.CustomSummaryProcess.Start
                    If e.RowHandle = 0 Then
                        e.TotalValue = 0
                    End If
                Case DevExpress.Data.CustomSummaryProcess.Calculate
                    'TagValue = TagValue
                Case DevExpress.Data.CustomSummaryProcess.Finalize
                    If e.RowHandle > 0 And e.IsTotalSummary Then
                        If view.RowCount = 0 Then
                            e.TotalValue = 0
                        End If
                        e.TotalValue = (From row In Enumerable.Range(0, view.RowCount) Select view.GetRowCellValue(row, summaryItem.FieldName)).Distinct.Count
                    End If
            End Select
        End Sub

#End Region

#Region "Expression Summary"
        Public Shared Function GetExpressionSummaryTotal(gsi As GridColumnSummaryItemEx) As Object
            Dim gv As GridView = gsi.View
            Dim expression As String = Convert.ToString(gsi.Info)
            'Dim cr As CriteriaOperator = CriteriaOperator.Parse("SumTop([V1]+1 ,10) + Sum([V1])")
            Dim cr As CriteriaOperator = SummaryExpressionCriteriaVisitor.Fix(CriteriaOperator.Parse(expression), gsi.View.DataController.Helper.DescriptorCollection)

            Dim customFunctions As List(Of ICustomFunctionOperator) = SummaryExpressionFunction.GetExpressionFunctions(gsi.View.DataController)

            Dim ev As New DevExpress.Data.Filtering.Helpers.ExpressionEvaluator(gsi.View.DataController.Helper.DescriptorCollection, cr, False, customFunctions)

            Return ev.Evaluate(gsi.View.DataController.Helper.List)
        End Function

        Public Shared Sub GetExpressionSummary(ByVal view As GridView, ByVal e As CustomSummaryEventArgs)
            Dim summaryItem As GridColumnSummaryItemEx = TryCast(e.Item, GridColumnSummaryItemEx)
            If Not e.IsTotalSummary OrElse Not summaryItem.SummaryTypeEx = SummaryItemTypeEx2.Expression Then Return
            Select Case e.SummaryProcess
                Case DevExpress.Data.CustomSummaryProcess.Start
                    If e.RowHandle = 0 Then
                        e.TotalValue = 0
                    End If
                Case DevExpress.Data.CustomSummaryProcess.Calculate
                    'TagValue = TagValue
                Case DevExpress.Data.CustomSummaryProcess.Finalize
                    If e.RowHandle > 0 And e.IsTotalSummary Then
                        If view.RowCount = 0 Then
                            e.TotalValue = 0
                        Else
                            Try
                                e.TotalValue = GetExpressionSummaryTotal(summaryItem)
                            Catch ex As Exception
                            End Try
                        End If
                    End If
            End Select
        End Sub

#End Region

#Region "TopBottom"
        Public Shared Function GetTopBottomSummaryTotal(gsi As GridColumnSummaryItemEx) As Object
            Dim lst As IEnumerable(Of Object) = Nothing
            Dim gv As GridView = gsi.View
            Dim st As SummaryItemTypeEx2 = gsi.SummaryTypeEx
            Dim fieldName As String = gsi.FieldName
            Dim cnt As Integer = gv.RowCount
            Dim x As Integer = Convert.ToInt32(gsi.Info)
            If SummaryItemTypeHelperEx.IsPercent(gsi.SummaryTypeEx) Then
                x = Convert.ToInt32(Math.Floor(cnt * x / 100))
            End If
            If x = 0 Then
                x = 1
            End If
            If SummaryItemTypeHelperEx.IsTop(st) Then
                lst = From row In Enumerable.Range(0, cnt) Select q = gv.GetRowCellValue(row, fieldName) Order By q Descending Take x
            ElseIf SummaryItemTypeHelperEx.IsButtom(st)
                lst = From row In Enumerable.Range(0, cnt) Select q = gv.GetRowCellValue(row, fieldName) Order By q Ascending Take x
            End If
            If SummaryItemTypeHelperEx.IsSum(st) Then
                If gv.Columns(fieldName).ColumnType Is GetType(TimeSpan) Then
                    Return TimeSpan.FromSeconds(lst.Sum(Function(s) DirectCast(s, TimeSpan).TotalSeconds))
                End If
                Return lst.Sum(Function(s) Convert.ToDecimal(s))
            ElseIf SummaryItemTypeHelperEx.IsAvg(st) Then
                If gv.Columns(fieldName).ColumnType Is GetType(TimeSpan) Then
                    Return TimeSpan.FromSeconds(lst.Average(Function(s) DirectCast(s, TimeSpan).TotalSeconds))
                End If
                Return lst.Average(Function(s) Convert.ToDecimal(s))
            End If
            Return Nothing
        End Function

        Public Shared Sub GetTopBottomSummary(ByVal view As GridView, ByVal e As CustomSummaryEventArgs)
            Dim summaryItem As GridColumnSummaryItemEx = TryCast(e.Item, GridColumnSummaryItemEx)
            If Not e.IsTotalSummary OrElse Not SummaryItemTypeHelperEx.IsTopButtom(summaryItem.SummaryTypeEx) Then Return
            Select Case e.SummaryProcess
                Case DevExpress.Data.CustomSummaryProcess.Start
                    If e.RowHandle = 0 Then
                        e.TotalValue = 0
                    End If
                Case DevExpress.Data.CustomSummaryProcess.Calculate
                    'TagValue = TagValue
                Case DevExpress.Data.CustomSummaryProcess.Finalize
                    If e.RowHandle > 0 And e.IsTotalSummary Then
                        If view.RowCount = 0 Then
                            e.TotalValue = 0
                        Else
                            e.TotalValue = GetTopBottomSummaryTotal(summaryItem)
                        End If
                    End If
            End Select
        End Sub

#End Region

#End Region

#Region "Can Apply Summary"
        Public Shared Function CanApplySumSummary(col As GridColumn) As Boolean
            Return col.ColumnType Is GetType(TimeSpan) OrElse SummaryItemTypeHelper.CanApplySummary(SummaryItemType.Sum, col.ColumnType)
        End Function

        Public Shared Function CanApplyAvgSummary(col As GridColumn) As Boolean
            Return col.ColumnType Is GetType(TimeSpan) OrElse SummaryItemTypeHelper.CanApplySummary(SummaryItemType.Average, col.ColumnType)
        End Function

        Public Shared Function CanApplySummary(st As SummaryItemTypeEx2, col As GridColumn) As Boolean
            If st = SummaryItemTypeEx2.None Then Return True
            Select Case st
                Case SummaryItemTypeEx2.Min, SummaryItemTypeEx2.Max
                    Return True
                Case Else
                    If SummaryItemTypeHelperEx.IsSumOrAvg(st) Then
                        Return col.ColumnType Is GetType(TimeSpan) OrElse SummaryItemTypeHelper.CanApplySummary(SummaryItemType.Sum, col.ColumnType)
                    End If
            End Select
            Return True
        End Function
#End Region

#Region "Helpers"
        Public Shared Function GetSummaryTypeCaption(st As SummaryItemTypeEx2, col As GridColumn, Optional info As Object = Nothing) As String
            If st = SummaryItemTypeEx2.None Then Return "None"
            Dim caption As String = Nothing
            info = GridColumnSummaryItemEx.FixSummaryInfoEx(st, info)
            Select Case st
                Case SummaryItemTypeEx2.Expression
                    caption = "Expression"
                Case SummaryItemTypeEx2.Average
                    caption = "Average"
                Case SummaryItemTypeEx2.Count
                    caption = "Count"
                Case SummaryItemTypeEx2.Max
                    caption = "Max"
                Case SummaryItemTypeEx2.Min
                    caption = "Min"
                Case SummaryItemTypeEx2.Sum
                    caption = "Sum"
                Case SummaryItemTypeEx2.UniqueValuesCount
                    caption = "Unique Values"
                Case SummaryItemTypeEx2.TopXPercentAvg
                    caption = String.Format("Top {0}% Avg", info)
                Case SummaryItemTypeEx2.TopXPercentSum
                    caption = String.Format("Top {0}% Sum", info)
                Case SummaryItemTypeEx2.BottomXPercentAvg
                    caption = String.Format("Bottom {0}% Avg", info)
                Case SummaryItemTypeEx2.BottomXPercentSum
                    caption = String.Format("Bottom {0}% Sum", info)
                Case SummaryItemTypeEx2.TopXAvg
                    caption = String.Format("Top {0} Avg", info)
                Case SummaryItemTypeEx2.TopXSum
                    caption = String.Format("Top {0} Sum", info)
                Case SummaryItemTypeEx2.BottomXAvg
                    caption = String.Format("Bottom {0} Avg", info)
                Case SummaryItemTypeEx2.BottomXSum
                    caption = String.Format("Bottom {0} Sum", info)
            End Select
            Return caption
        End Function

        Public Shared Function GetSummaryTypeDisplayFormat(st As SummaryItemTypeEx2, col As GridColumn, Optional info As Object = Nothing) As String
            If st = SummaryItemTypeEx2.None Then Return ""
            Dim displayFormat As String = GetSummaryTypeCaption(st, col, info)
            If displayFormat Is Nothing Then Return Nothing
            If col.ColumnType Is GetType(TimeSpan) AndAlso (SummaryItemTypeHelperEx.IsMinOrMax(st) OrElse SummaryItemTypeHelperEx.IsSumOrAvg(st)) Then
                displayFormat &= ": " & TimeSpanFormatHelper.DEFAULT_DISPLAY_FORMAT
            Else
                displayFormat &= ": {0}"
            End If
            Return displayFormat
        End Function
#End Region
    End Class
End Namespace
