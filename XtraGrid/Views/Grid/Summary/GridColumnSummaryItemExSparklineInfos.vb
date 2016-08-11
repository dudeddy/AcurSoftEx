Imports AcurSoft.Data
Imports AcurSoft.XtraGrid.Views.Grid.Extenders
Imports DevExpress.Data
Imports DevExpress.Utils.Serializing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports DevExpress.Sparkline
Imports DevExpress.Data.Helpers
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Data.Filtering
Imports AcurSoft.Data.Filtering
Imports AcurSoft.Data.Filtering.Summary

Namespace AcurSoft.XtraGrid.Views.Grid.Summary

    <Serializable()>
    Public Class GridColumnSummaryItemExSparklineInfos

        <XtraSerializableProperty, DefaultValue(SparklineViewType.Line)>
        Public Property ViewType As SparklineViewType = SparklineViewType.Line

        <XtraSerializableProperty>
        Public Property UseExpression As Boolean
        <XtraSerializableProperty>
        Public Property Expression As String


        <XtraSerializableProperty>
        Public Property UseOrderExpression As Boolean
        <XtraSerializableProperty, DefaultValue(ColumnSortOrder.None)>
        Public Property OrderDirection As DevExpress.Data.ColumnSortOrder = ColumnSortOrder.None
        <XtraSerializableProperty>
        Public Property OrderExpression As String


        '<XtraSerializableProperty>
        'Public Property HighlightStartPoint As Boolean
        '<XtraSerializableProperty>
        'Public Property HighlightEndPoint As Boolean
        <XtraSerializableProperty>
        Public Property HighlightMaxPoint As Boolean
        <XtraSerializableProperty>
        Public Property HighlightMinPoint As Boolean
        <XtraSerializableProperty>
        Public Property HighlightNegativePoints As Boolean

        <XtraSerializableProperty, DefaultValue(-16777216)>
        Public Property LineColor As Integer = Color.Black.ToArgb
        <XtraSerializableProperty, DefaultValue(-16744448)>
        Public Property MaxPointColor As Integer = Color.Green.ToArgb
        <XtraSerializableProperty, DefaultValue(-65536)>
        Public Property MinPointColor As Integer = Color.Red.ToArgb
        Public Function CreateView() As SparklineViewBase
            Return Me.CreateView(Me.ViewType)
        End Function
        Public Function CreateView(viewType As SparklineViewType) As SparklineViewBase

            Dim view As SparklineViewBase = SparklineViewBase.CreateView(Me.ViewType)
            With view
                '.HighlightStartPoint = Me.HighlightStartPoint
                '.HighlightEndPoint = Me.HighlightEndPoint
                .HighlightMaxPoint = Me.HighlightMaxPoint
                .HighlightMinPoint = Me.HighlightMinPoint

                .Color = Color.FromArgb(Me.LineColor)
                .MaxPointColor = Color.FromArgb(Me.MaxPointColor)
                .MinPointColor = Color.FromArgb(Me.MinPointColor)
            End With
            Select Case Me.ViewType
                Case SparklineViewType.Line
                    With DirectCast(view, LineSparklineView)
                        .HighlightNegativePoints = Me.HighlightNegativePoints
                    End With
                Case SparklineViewType.Area
                    With DirectCast(view, AreaSparklineView)
                        .HighlightNegativePoints = Me.HighlightNegativePoints
                    End With
                Case SparklineViewType.Bar
                    With DirectCast(view, BarSparklineView)
                        .HighlightNegativePoints = Me.HighlightNegativePoints
                    End With
                Case SparklineViewType.WinLoss
                    With DirectCast(view, WinLossSparklineView)
                        '.HighlightNegativePoints = Me.HighlightNegativePoints
                    End With
            End Select
            Return view
        End Function

        <NonSerialized>
        Private _Sparkline As SparklineEdit
        Public Function GetSparkline() As SparklineEdit
            If _Sparkline Is Nothing Then
                _Sparkline = New SparklineEdit
                '_Sparkline.EditValue = New Double() {2, 4, 5, 1, -1, -2, -1, 2, 4, 5, 6, 3, 5, 4, 8, -1, 6}

                ' Create an Area view and assign it to the sparkline.

                ' Customize area appearance.
                'view.Color = Color.Aqua
                'view.AreaOpacity = 50

                ' Show markerks.
                'view.SetSizeForAllMarkers(10)
            End If
            If _SummaryItem IsNot Nothing Then
                _Sparkline.EditValue = Me.GetSparklineData()
            End If

            _Sparkline.Properties.View = Me.CreateView()

            Return _Sparkline
        End Function


        <NonSerialized>
        Private _SummaryItem As GridColumnSummaryItemEx
        Public Sub SetSummary(si As GridColumnSummaryItemEx)
            _SummaryItem = si
        End Sub

        Public Sub New()

        End Sub


        Public Sub New(si As GridColumnSummaryItemEx)
            _SummaryItem = si
        End Sub

        Public Function GetSparklineData(
                    useExpression As Boolean,
                    expression As String,
                    orderDirection As ColumnSortOrder,
                    useOrderExpression As Boolean,
                    orderExpression As String) As Object

            Dim data As IEnumerable(Of Double) = Nothing
            Dim exp As String = Nothing
            Dim dc As BaseGridController = _SummaryItem.View.DataController
            Dim cnt As Integer = dc.ListSourceRowCount
            Dim h As BaseDataControllerHelper = dc.Helper
            If useExpression Then
                If String.IsNullOrEmpty(expression) Then
                    exp = _SummaryItem.FieldName
                Else
                    exp = expression
                End If
            Else
                exp = _SummaryItem.FieldName
            End If
            Dim pd As PropertyDescriptor = h.DescriptorCollection.Find(exp.Trim().TrimStart("["c).TrimEnd("]"c), True)
            Dim getvalue As Func(Of Integer, Double) = Nothing
            Try


                If pd Is Nothing Then
                    Dim ev As New ExpressionEvaluator(h.DescriptorCollection, SummaryExpressionCriteriaVisitor.Fix(CriteriaOperator.Parse(exp), h.DescriptorCollection), False)
                    getvalue = Function(q)
                                   Dim o As Object = h.GetRow(q)
                                   If o Is Nothing Then Return 0
                                   o = ev.Evaluate(o)
                                   If o Is Nothing Then Return 0
                                   If TypeOf o Is TimeSpan Then
                                       Return DirectCast(o, TimeSpan).TotalSeconds
                                   ElseIf o.ToString.IsNumeric
                                       Return Convert.ToDouble(o)
                                   End If
                                   Return 0
                               End Function

                    'eval
                Else
                    Dim index As Integer = h.DescriptorCollection.IndexOf(pd)
                    getvalue =
                        Function(q)
                            Dim o As Object = h.GetRowValue(q, index)
                            If o Is Nothing Then Return 0
                            If TypeOf o Is TimeSpan Then
                                Return DirectCast(o, TimeSpan).TotalSeconds
                            ElseIf o.ToString.IsNumeric
                                Return Convert.ToDouble(o)
                            End If
                            Return 0
                        End Function
                End If
                If getvalue Is Nothing Then
                    Return Nothing
                Else
                    If orderDirection = ColumnSortOrder.None Then
                        data = From row In Enumerable.Range(0, cnt) Select getvalue(row)
                    Else
                        If (Not useOrderExpression) OrElse (useOrderExpression AndAlso String.IsNullOrEmpty(orderExpression)) Then
                            If orderDirection = ColumnSortOrder.Ascending Then
                                data = From row In Enumerable.Range(0, cnt) Select q = getvalue(row) Order By q Ascending
                            Else
                                data = From row In Enumerable.Range(0, cnt) Select q = getvalue(row) Order By q Descending
                            End If
                        ElseIf useOrderExpression AndAlso Not String.IsNullOrEmpty(orderExpression)
                            Dim orderGetvalue As Func(Of Integer, Object) = Nothing
                            Dim ev As New ExpressionEvaluator(h.DescriptorCollection, SummaryExpressionCriteriaVisitor.Fix(CriteriaOperator.Parse(orderExpression), h.DescriptorCollection), False)
                            orderGetvalue = Function(q)
                                                Return ev.Evaluate(h.GetRow(q))
                                            End Function

                            If orderDirection = ColumnSortOrder.Ascending Then
                                data = From row In Enumerable.Range(0, cnt) Select x = row, q = getvalue(row) Order By orderGetvalue(x) Ascending Select q
                            Else
                                data = From row In Enumerable.Range(0, cnt) Select x = row, q = getvalue(row) Order By orderGetvalue(x) Descending Select q
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                data = Nothing
            End Try
            If data Is Nothing Then
                Return Nothing
            Else
                Return data.ToList
            End If
        End Function


        Public Function GetSparklineData() As Object
            Return Me.GetSparklineData(Me.UseExpression, Me.Expression, Me.OrderDirection, Me.UseOrderExpression, Me.OrderExpression)
        End Function

    End Class
End Namespace
