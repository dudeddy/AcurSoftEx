Imports System.ComponentModel
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Data.Helpers
Imports DevExpress.XtraGrid.Views.Base

Public Class SummaryExpressionFunction
    Implements ICustomFunctionOperator
    Public Enum TimeSpanPrecision
        Seconds
        Minutes
        Hours
        Days
        None
    End Enum


    Public Enum AggregateTypeEnum
        Sum
        Avg
        Count
        'Unique
        DistinctCount
        DistinctSum
        DistinctAvg
        ValueAt
    End Enum

    Public ReadOnly Property DataController As BaseListSourceDataController
    Public ReadOnly Property IsTop As Boolean
    Public ReadOnly Property IsRank As Boolean
    Public ReadOnly Property IsDistinct As Boolean
    Public ReadOnly Property AggregateType As AggregateTypeEnum

#Region "Constructors"


    Public Sub New(dataController As BaseListSourceDataController, aggregateType As AggregateTypeEnum, isTop As Boolean)
        MyBase.New()
        Me.DataController = dataController
        Me.IsTop = isTop
        Me.IsRank = False
        Select Case aggregateType
            Case AggregateTypeEnum.DistinctCount
                Me.IsDistinct = True
                Me.AggregateType = AggregateTypeEnum.Count
            Case AggregateTypeEnum.DistinctSum
                Me.IsDistinct = True
                Me.AggregateType = AggregateTypeEnum.Sum
            Case AggregateTypeEnum.DistinctAvg
                Me.IsDistinct = True
                Me.AggregateType = AggregateTypeEnum.Avg
            Case Else
                Me.AggregateType = aggregateType
        End Select
    End Sub

    'Rank
    Public Sub New(dataController As BaseListSourceDataController)
        MyBase.New()
        Me.DataController = dataController
        Me.IsTop = True
        Me.IsRank = True
        Me.AggregateType = AggregateTypeEnum.Sum
    End Sub

#End Region

    Public Overridable ReadOnly Property Name As String Implements ICustomFunctionOperator.Name
        Get
            If Me.AggregateType = AggregateTypeEnum.ValueAt Then
                Return "ValueAt"
            ElseIf Me.IsDistinct AndAlso Me.AggregateType = AggregateTypeEnum.Count Then
                Return "DistinctCount"
            ElseIf Not Me.IsDistinct AndAlso Me.AggregateType = AggregateTypeEnum.Count Then
                Return If(Me.IsTop, "CountTop", "CountBottom")
            ElseIf Me.IsDistinct AndAlso Me.AggregateType = AggregateTypeEnum.Sum Then
                Return "DistinctSum"
            ElseIf Me.IsDistinct AndAlso Me.AggregateType = AggregateTypeEnum.Avg Then
                Return "DistinctAvg"
            ElseIf Me.IsRank Then
                Return "Rank"
            End If

            Return If(Me.AggregateType = AggregateTypeEnum.Avg, "Avg", "Sum") & If(Me.IsTop, "Top", "Bottom")
        End Get
    End Property


    Public Function Evaluate(ParamArray operands() As Object) As Object Implements ICustomFunctionOperator.Evaluate
        Dim expression As String = operands(0).ToString()
        Dim timePrecision As TimeSpanPrecision = TimeSpanPrecision.Seconds
        If Me.AggregateType = AggregateTypeEnum.ValueAt Then
            Dim row As Integer = 0
            If operands.Count > 1 Then
                If operands(1).ToString().IsNumeric Then
                    row = Convert.ToInt32(operands(1))
                End If
                If operands.Count > 2 Then
                    timePrecision = GetTimePrecision(operands(2))
                End If
            End If
            Return GetValueAt(Me.DataController, row, expression, timePrecision)
        End If

        If Me.IsDistinct AndAlso Me.AggregateType = AggregateTypeEnum.Count Then
            If operands.Count > 1 Then
                timePrecision = GetTimePrecision(operands(1))
            End If
            Return GetUniqueCount(Me.DataController, expression, timePrecision)
        End If

        Dim x As Decimal = 0
        Dim isPercent As Boolean = False
        If Me.IsDistinct Then
            If operands.Count > 1 Then
                If operands(1).ToString().IsNumeric Then
                    x = Convert.ToDecimal(operands(1))
                    If operands.Count > 2 Then
                        If TypeOf operands(2) Is Boolean Then
                            isPercent = Convert.ToBoolean(operands(2))
                        ElseIf TypeOf operands(2) Is String Then
                            timePrecision = GetTimePrecision(operands(2))
                        End If
                        If operands.Count > 3 AndAlso TypeOf operands(3) Is Boolean Then
                            timePrecision = GetTimePrecision(operands(3))
                        End If
                    End If
                ElseIf TypeOf operands(1) Is String Then
                    timePrecision = GetTimePrecision(operands(1))
                End If
            Else
                x = Me.DataController.ListSourceRowCount
            End If
        Else
            x = Convert.ToDecimal(operands(1))
            'If Me.AggregateType = AggregateTypeEnum.Count Then
            '    isPercent = True
            '    If operands.Count > 2 Then
            '        timePrecision = GetTimePrecision(operands(2))
            '    End If

            'Else
            If operands.Count > 2 AndAlso operands(2) IsNot Nothing Then
                If TypeOf operands(2) Is Boolean Then
                    isPercent = Convert.ToBoolean(operands(2))
                    If operands.Count > 3 Then
                        timePrecision = GetTimePrecision(operands(3))
                    End If
                ElseIf TypeOf operands(2) Is String Then
                    timePrecision = GetTimePrecision(operands(2))
                End If
            End If

        End If
        'End If

        Dim isTop As Boolean = Me.IsTop
        If Me.IsRank OrElse Me.IsDistinct Then
            isTop = x > 0
        ElseIf Me.IsTop Then
            isTop = x > 0
        Else
            isTop = x < 0
        End If
        x = Math.Abs(x)
        If Me.IsRank Then
            Return GetRank(Me.DataController, expression, isTop, x, isPercent, timePrecision)
        Else
            'If Me.AggregateType = AggregateTypeEnum.Count Then
            '    Return GetCountTopBotton(Me.DataController, expression, isTop, x, Me.IsDistinct, timePrecision)
            'End If
            Return GetSumTopBotton(Me.DataController, expression, Me.AggregateType, isTop, x, isPercent, Me.IsDistinct, timePrecision)
        End If

    End Function


    Public Function ResultType(ParamArray operands() As Type) As Type Implements ICustomFunctionOperator.ResultType
        Return GetType(Object)
    End Function

#Region "Shared Helpers"
    Public Shared Function GetValueAt(dataController As BaseListSourceDataController,
                                      rowIndex As Integer,
                                      expression As String,
                                      timePrecision As TimeSpanPrecision) As Object
        Dim fnc As Func(Of Integer, Object) = GetGetValue(dataController, expression, True, timePrecision)
        Return fnc(rowIndex)
    End Function

    Public Shared Function GetGetValue(dataController As BaseListSourceDataController,
                                    expression As String,
                                    isDistinct As Boolean,
                                    timePrecision As TimeSpanPrecision) As Func(Of Integer, Object)
        Dim h As BaseDataControllerHelper = dataController.Helper
        Dim data As IEnumerable(Of Object) = Nothing
        Dim preGetValue As Func(Of Integer, Object) = Nothing

        Dim pd As PropertyDescriptor = h.DescriptorCollection.Find(expression.Trim().TrimStart("["c).TrimEnd("]"c), True)
        If pd Is Nothing Then
            Dim ev As New ExpressionEvaluator(h.DescriptorCollection, SummaryExpressionCriteriaVisitor.Fix(CriteriaOperator.Parse(expression), h.DescriptorCollection), False)
            preGetValue = Function(i) ev.Evaluate(h.GetRow(i))
        Else
            Dim index As Integer = h.DescriptorCollection.IndexOf(pd)
            preGetValue = Function(i) h.GetRowValue(i, index)
        End If

        Return Function(i)
                   Dim o As Object = preGetValue(i)
                   If isDistinct Then
                       Dim timeAg As Func(Of Object, Double) = GetAgregateFunc(timePrecision <> TimeSpanPrecision.None AndAlso TypeOf o Is TimeSpan, timePrecision)
                       Return timeAg(o)
                   End If
                   Return o
               End Function
    End Function


    Public Shared Function GetAggregateData(dataController As BaseListSourceDataController,
                                            expression As String,
                                            isTop As Boolean,
                                            x As Decimal,
                                            isPercent As Boolean,
                                            isRank As Boolean,
                                            isDistinct As Boolean,
                                            timePrecision As TimeSpanPrecision) As IEnumerable(Of Object)

        Dim h As BaseDataControllerHelper = dataController.Helper
        Dim data As IEnumerable(Of Object) = Nothing
        Dim getValue As Func(Of Integer, Object) = GetGetValue(dataController, expression, isDistinct, timePrecision)

        Dim cnt As Integer = h.Count
        If isPercent Then
            x = Convert.ToInt32(Math.Round(cnt * x / 100, 0))
        End If
        x = If(x = 0, 1, x)
        Dim taken As Integer = Convert.ToInt32(x)
        Dim skiped As Integer = 0
        If isRank Then
            skiped = taken - 1
        End If

        If isTop Then
            If isDistinct Then
                data = From row In Enumerable.Range(0, cnt) Select q = getValue(row) Distinct Order By q Descending Take taken Skip skiped
            Else
                data = From row In Enumerable.Range(0, cnt) Select q = getValue(row) Order By q Descending Take taken Skip skiped
            End If
        Else
            If isDistinct Then
                data = From row In Enumerable.Range(0, cnt) Select q = getValue(row) Distinct Order By q Ascending Take taken Skip skiped
            Else
                data = From row In Enumerable.Range(0, cnt) Select q = getValue(row) Order By q Ascending Take taken Skip skiped
            End If
        End If
        Return data
    End Function

    Public Shared Function GetRank(
                    dataController As BaseListSourceDataController,
                    expression As String,
                    isTop As Boolean,
                    x As Decimal,
                    isPercent As Boolean,
                    Optional timePrecision As TimeSpanPrecision = TimeSpanPrecision.Minutes) As Object
        Dim data As IEnumerable(Of Object) = GetAggregateData(dataController, expression, isTop, x, isPercent, True, False, TimeSpanPrecision.None)
        Dim o As Object = data.FirstOrDefault
        If o Is Nothing Then Return Nothing
        Dim isTimeSpan As Boolean = TypeOf data.FirstOrDefault Is TimeSpan
        Dim agregateFunc As Func(Of Object, Double) = GetAgregateFunc(isTimeSpan, timePrecision)
        Return GetFixedTotal(agregateFunc(o), isTimeSpan, timePrecision)
    End Function

    Public Shared Function GetAgregateFunc(isTimeSpan As Boolean, timePrecision As TimeSpanPrecision) As Func(Of Object, Double)
        'Dim isTimeSpan As Boolean = TypeOf Data.FirstOrDefault Is TimeSpan
        Dim agregateFunc As Func(Of Object, Double) = Function(q) Convert.ToDecimal(q)

        If isTimeSpan Then
            Select Case timePrecision
                Case TimeSpanPrecision.Days
                    agregateFunc = Function(q)
                                       Dim tot As Double = DirectCast(q, TimeSpan).TotalDays
                                       Return If(tot < 0, Math.Ceiling(tot), Math.Floor(tot))
                                   End Function

                Case TimeSpanPrecision.Hours
                    agregateFunc = Function(q)
                                       Dim tot As Double = DirectCast(q, TimeSpan).TotalHours
                                       Return If(tot < 0, Math.Ceiling(tot), Math.Floor(tot))
                                   End Function

                Case TimeSpanPrecision.Minutes
                    agregateFunc = Function(q)
                                       Dim tot As Double = DirectCast(q, TimeSpan).TotalMinutes
                                       Return If(tot < 0, Math.Ceiling(tot), Math.Floor(tot))
                                   End Function
                Case Else
                    agregateFunc = Function(q)
                                       Dim tot As Double = DirectCast(q, TimeSpan).TotalSeconds
                                       Return If(tot < 0, Math.Ceiling(tot), Math.Floor(tot))
                                   End Function
            End Select
        End If
        Return agregateFunc
    End Function

    Public Shared Function GetFixedTotal(o As Object, isTimeSpan As Boolean, timePrecision As TimeSpanPrecision) As Object
        If isTimeSpan Then
            Select Case timePrecision
                Case TimeSpanPrecision.Days
                    Return TimeSpan.FromDays(Convert.ToDouble(o))
                Case TimeSpanPrecision.Hours
                    Return TimeSpan.FromHours(Convert.ToDouble(o))
                Case TimeSpanPrecision.Minutes
                    Return TimeSpan.FromMinutes(Convert.ToDouble(o))
                Case TimeSpanPrecision.Seconds
                    Return TimeSpan.FromSeconds(Convert.ToDouble(o))
            End Select
        End If
        Return o
    End Function

    Public Shared Function GetUniqueCount(
                    dataController As BaseListSourceDataController,
                    expression As String,
                    Optional timePrecision As TimeSpanPrecision = TimeSpanPrecision.Minutes) As Object
        Return GetSumTopBotton(dataController, expression, AggregateTypeEnum.Count, False, 100, True, True, timePrecision)
    End Function

    Public Shared Function GetSumTopBotton(
                    dataController As BaseListSourceDataController,
                    expression As String,
                    aggregateType As AggregateTypeEnum,
                    isTop As Boolean,
                    x As Decimal,
                    isPercent As Boolean,
                    isDistinct As Boolean,
                    Optional timePrecision As TimeSpanPrecision = TimeSpanPrecision.Seconds) As Object
        Dim data As IEnumerable(Of Object) = GetAggregateData(dataController, expression, isTop, x, isPercent, False, isDistinct, timePrecision)
        If aggregateType = AggregateTypeEnum.Count Then
            Return data.Count
        Else
            Dim isTimeSpan As Boolean = TypeOf data.FirstOrDefault Is TimeSpan
            Dim o As Object = Nothing
            Dim agregateFunc As Func(Of Object, Double) = GetAgregateFunc(isTimeSpan, timePrecision)
            Select Case aggregateType
                Case AggregateTypeEnum.Sum
                    o = data.Sum(Function(s)
                                     Return agregateFunc(s)
                                 End Function)
                Case AggregateTypeEnum.Avg
                    o = data.Average(Function(s)
                                         Return agregateFunc(s)
                                     End Function)
                    'Case AggregateTypeEnum.Count
                    '    o = data.Count(Function(s)
                    '                       Return agregateFunc(s)
                    '                   End Function)

            End Select
            Return GetFixedTotal(o, isTimeSpan, timePrecision)


        End If
    End Function

    'Public Shared Function GetCountTopBotton(
    '                dataController As BaseListSourceDataController,
    '                expression As String,
    '                isTop As Boolean,
    '                x As Decimal,
    '                isDistinct As Boolean,
    '                Optional timePrecision As TimeSpanPrecision = TimeSpanPrecision.Seconds) As Object
    '    Dim data As IEnumerable(Of Object) = GetAggregateData(dataController, expression, isTop, 100, True, False, isDistinct, timePrecision)
    '    Dim isTimeSpan As Boolean = TypeOf data.FirstOrDefault Is TimeSpan
    '    Dim agregateFunc As Func(Of Object, Double) = GetAgregateFunc(isTimeSpan, timePrecision)
    '    Dim sum As Object = GetFixedTotal(data.Sum(Function(s) agregateFunc(s)), isTimeSpan, timePrecision)
    '    If isTimeSpan Then
    '        Dim sumTime As Double = 0

    '        Select Case timePrecision
    '            Case TimeSpanPrecision.Days
    '                sumTime = DirectCast(sum, TimeSpan).TotalDays
    '            Case TimeSpanPrecision.Hours
    '                sumTime = DirectCast(sum, TimeSpan).TotalHours
    '            Case TimeSpanPrecision.Minutes
    '                sumTime = DirectCast(sum, TimeSpan).TotalMinutes
    '            Case Else
    '                sumTime = DirectCast(sum, TimeSpan).TotalSeconds
    '        End Select
    '        sumTime = sumTime * x / 100
    '        data = data.Where(Function(q) DirectCast(q, Double) <= sumTime)
    '    Else
    '        Dim sumDecimal As Decimal = Convert.ToDecimal(sum) * x / 100
    '        data = data.Where(Function(q) DirectCast(q, Decimal) <= sumDecimal)
    '    End If

    '    Return data.Count
    'End Function

    Public Shared Function GetTimePrecision(o As Object) As TimeSpanPrecision
        Dim timePrecision As TimeSpanPrecision = TimeSpanPrecision.Seconds

        If TypeOf o Is String Then
            Dim tp As String = Convert.ToString(o).ToString().Trim.ToUpper()
            Select Case tp
                Case "D"
                    timePrecision = TimeSpanPrecision.Days
                Case "H"
                    timePrecision = TimeSpanPrecision.Hours
                Case "M"
                    timePrecision = TimeSpanPrecision.Minutes
                Case "S"
                    timePrecision = TimeSpanPrecision.Seconds
            End Select
        End If
        Return timePrecision
    End Function

    Public Shared Function GetExpressionFunctions(dataController As BaseListSourceDataController) As List(Of ICustomFunctionOperator)
        Dim l As New List(Of ICustomFunctionOperator)
        'l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.Count, True))
        'l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.Count, False))
        l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.Sum, True))
        l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.Sum, False))
        l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.Avg, True))
        l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.Avg, False))
        l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.DistinctCount, False))
        l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.DistinctSum, True))
        l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.DistinctAvg, True))
        l.Add(New SummaryExpressionFunction(dataController, AggregateTypeEnum.ValueAt, True))
        l.Add(New SummaryExpressionFunction(dataController))
        Return l
    End Function
    Public Shared Function IsExpressionFunction(functionName As String) As Boolean
        Return {"ValueAt".ToUpper, "DistinctCount".ToUpper, "DistinctSum".ToUpper, "DistinctAvg".ToUpper, "RANK", "SUMTOP", "SUMBOTTOM", "AVGTOP", "AVGBOTTOM"}.Contains(functionName.Trim.ToUpper)
    End Function


#End Region

End Class

