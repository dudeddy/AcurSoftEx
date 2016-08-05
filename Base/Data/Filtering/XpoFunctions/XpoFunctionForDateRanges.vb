Imports System.Linq.Expressions
Imports System.Reflection
Imports DevExpress.Data.Filtering

Namespace AcurSoft.Data.Filtering
    Public Class XpoFunctionForDateRanges
        Inherits XpoFunctionForDates

        Public Overrides Property Kind As DateFunctionKind = DateFunctionKind.GetRange

        'Public ReadOnly Property ChooseKind As DateStartKind
        '    Get
        '        If Convert.ToInt32(Me.Kind).HasBitFlag(Convert.ToInt32(DateStartKind.Year)) AndAlso Convert.ToInt32(Me.Kind).HasBitFlag(Convert.ToInt32(DateStartKind.Month)) Then
        '            Return DateStartKind.Year Or DateStartKind.Month
        '        ElseIf Convert.ToInt32(Me.Kind).HasBitFlag(Convert.ToInt32(DateStartKind.Year)) AndAlso Convert.ToInt32(Me.Kind).HasBitFlag(Convert.ToInt32(DateStartKind.Quarter)) Then
        '            Return DateStartKind.Year Or DateStartKind.Quarter
        '        ElseIf Convert.ToInt32(Me.Kind).HasBitFlag(Convert.ToInt32(DateStartKind.Year)) Then
        '            Return DateStartKind.Year

        '        End If

        '    End Get
        'End Property


        'Public Property StartFunctionName As String
        'Public Property EndFunctionName As String

        Public Property StartFunction As XpoFunctionForDates
        Public Property EndFunction As XpoFunctionForDates

        Public ReadOnly Property HasBeweenFunctions As Boolean
            Get
                Return StartFunction IsNot Nothing AndAlso EndFunction IsNot Nothing
            End Get
        End Property



        Public Sub New(mi As MethodInfo)
            Me.New(mi, mi.Name)
        End Sub
        Public Sub New(d As [Delegate], functionName As String)
            MyBase.New(d, functionName)
        End Sub

        Public Sub New(exp As LambdaExpression, functionName As String)
            MyBase.New(exp, functionName)
        End Sub
        Public Sub New(mi As MethodInfo, functionName As String)
            MyBase.New(mi, functionName)
        End Sub

        Public ReadOnly Property RangeCriteria As DateRangeCriteria
        'Public ReadOnly Property RangeCriteria As CriteriaOperator
        'Public ReadOnly Property RangeCriteriaBetween As CriteriaOperator

        'Public ReadOnly Property RangeCriteriaBetweenText As String
        '    Get
        '        If Me.HasBeweenFunctions Then
        '            Return Me.RangeCriteriaBetween.ToString
        '        End If
        '        Return ""
        '    End Get
        'End Property


        'Public ReadOnly Property LabelValue As String

        'Public Sub UpdateRangeCriteria(CurrentDate As Date, propertyName As String, config As Integer, year As Integer, quarter As Integer, month As Integer, monthWeekNumber As Integer, firstDayOfWeek As Integer)


        '    Dim yearCr As CriteriaOperator = Nothing
        '    If year = 0 Then
        '        year = Date.Today.Year
        '        yearCr = CriteriaOperator.Parse("ThisYear()")

        '    Else
        '        yearCr = New ConstantValue(year)
        '    End If
        '    Dim quarterCr As CriteriaOperator = Nothing
        '    If quarter = 0 Then
        '        quarter = Date.Today.GetQuarter
        '        quarterCr = CriteriaOperator.Parse("ThisQuarter()")
        '    Else
        '        quarterCr = New ConstantValue(quarter)
        '    End If

        '    Dim monthCr As CriteriaOperator = Nothing
        '    If month = 0 Then
        '        month = Date.Today.Month
        '        monthCr = CriteriaOperator.Parse("ThisMonth()")
        '    Else
        '        monthCr = New ConstantValue(month)
        '    End If

        '    'Dim monthWeekNumber As Integer = Convert.ToInt32(Me.LookUpEditMonthWeek.EditValue)
        '    Dim fullWeek As Boolean = Math.Abs(monthWeekNumber) > 1000
        '    monthWeekNumber = monthWeekNumber Mod 1000
        '    Dim monthWeekNumberCr As CriteriaOperator = New OperandValue(monthWeekNumber)
        '    Dim fullWeekCr As CriteriaOperator = New OperandValue(fullWeek)

        '    Dim firstDayOfWeekCr As CriteriaOperator = New OperandValue(firstDayOfWeek)

        '    'Dim _RangeCriteria As CriteriaOperator = Nothing

        '    If Me.Kind.HasFlag(DateFunctionKind.NeedMonthWeekNumber) Then
        '        _RangeCriteria = Me.ToCriteriaEx(CurrentDate, yearCr, monthCr, monthWeekNumberCr, fullWeekCr, firstDayOfWeekCr)
        '        _LabelValue = Me.Evaluate(CurrentDate, year, month, monthWeekNumber, fullWeek, firstDayOfWeek).ToString
        '        If Me.HasBeweenFunctions Then
        '            _RangeCriteriaBetween = New BetweenOperator With {
        '            .BeginExpression = New ConstantValue(Me.StartFunction.Evaluate(year, month, monthWeekNumber, fullWeek, firstDayOfWeek)),
        '            .EndExpression = New ConstantValue(Me.EndFunction.Evaluate(year, month, monthWeekNumber, fullWeek, firstDayOfWeek))
        '        }
        '        End If
        '    ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedMonth) Then
        '        _RangeCriteria = Me.ToCriteriaEx(CurrentDate, yearCr, monthCr)
        '        _LabelValue = Me.Evaluate(CurrentDate, year, month).ToString
        '        If Me.HasBeweenFunctions Then
        '            _RangeCriteriaBetween = New BetweenOperator With {
        '            .BeginExpression = New ConstantValue(Me.StartFunction.Evaluate(year, month)),
        '            .EndExpression = New ConstantValue(Me.EndFunction.Evaluate(year, month))
        '        }
        '        End If
        '    ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedQuarter) Then
        '        _RangeCriteria = Me.ToCriteriaEx(CurrentDate, yearCr, quarterCr)
        '        _LabelValue = Me.Evaluate(CurrentDate, year, quarter).ToString
        '        If Me.HasBeweenFunctions Then
        '            _RangeCriteriaBetween = New BetweenOperator With {
        '            .BeginExpression = New ConstantValue(Me.StartFunction.Evaluate(year, quarter)),
        '            .EndExpression = New ConstantValue(Me.EndFunction.Evaluate(year, quarter))
        '        }
        '        End If
        '    ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedYear) Then
        '        _RangeCriteria = Me.ToCriteriaEx(CurrentDate, yearCr)
        '        _LabelValue = Me.Evaluate(CurrentDate, year).ToString
        '        If Me.HasBeweenFunctions Then
        '            _RangeCriteriaBetween = New BetweenOperator With {
        '            .BeginExpression = New ConstantValue(Me.StartFunction.Evaluate(year)),
        '            .EndExpression = New ConstantValue(Me.EndFunction.Evaluate(year))
        '        }
        '        End If
        '    End If
        '    'If _RangeCriteriaBetween IsNot Nothing Then
        '    '    Me.LabelCriteriaBetween.Text = _RangeCriteriaBetween.ToString
        '    'End If
        '    'Me.LabelCriteria.Text = _RangeCriteria.ToString()

        '    'Return _RangeCriteria.ToString()
        'End Sub

        'Public Sub UpdateRangeCriteria(CurrentDate As Date, propertyName As String, config As Integer, year As Integer, quarter As Integer, month As Integer, monthWeekNumber As Integer, firstDayOfWeek As Integer)


        '    Dim yearCr As CriteriaOperator = Nothing
        '    If year = 0 Then
        '        year = Date.Today.Year
        '        yearCr = CriteriaOperator.Parse("ThisYear()")

        '    Else
        '        yearCr = New ConstantValue(year)
        '    End If
        '    Dim quarterCr As CriteriaOperator = Nothing
        '    If quarter = 0 Then
        '        quarter = Date.Today.GetQuarter
        '        quarterCr = CriteriaOperator.Parse("ThisQuarter()")
        '    Else
        '        quarterCr = New ConstantValue(quarter)
        '    End If

        '    Dim monthCr As CriteriaOperator = Nothing
        '    If month = 0 Then
        '        month = Date.Today.Month
        '        monthCr = CriteriaOperator.Parse("ThisMonth()")
        '    Else
        '        monthCr = New ConstantValue(month)
        '    End If

        '    'Dim monthWeekNumber As Integer = Convert.ToInt32(Me.LookUpEditMonthWeek.EditValue)
        '    Dim fullWeek As Boolean = Math.Abs(monthWeekNumber) > 1000
        '    monthWeekNumber = monthWeekNumber Mod 1000
        '    Dim monthWeekNumberCr As CriteriaOperator = New OperandValue(monthWeekNumber)
        '    Dim fullWeekCr As CriteriaOperator = New OperandValue(fullWeek)

        '    Dim firstDayOfWeekCr As CriteriaOperator = New OperandValue(firstDayOfWeek)

        '    'Dim _RangeCriteria As CriteriaOperator = Nothing

        '    If Me.Kind.HasFlag(DateFunctionKind.NeedMonthWeekNumber) Then
        '        _RangeCriteria = Me.ToCriteriaEx(CurrentDate, yearCr, monthCr, monthWeekNumberCr, fullWeekCr, firstDayOfWeekCr)
        '        _LabelValue = Me.Evaluate(CurrentDate, year, month, monthWeekNumber, fullWeek, firstDayOfWeek).ToString
        '        If Me.HasBeweenFunctions Then
        '            _RangeCriteriaBetween = New BetweenOperator With {
        '            .BeginExpression = New ConstantValue(Me.StartFunction.Evaluate(year, month, monthWeekNumber, fullWeek, firstDayOfWeek)),
        '            .EndExpression = New ConstantValue(Me.EndFunction.Evaluate(year, month, monthWeekNumber, fullWeek, firstDayOfWeek))
        '        }
        '        End If
        '    ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedMonth) Then
        '        _RangeCriteria = Me.ToCriteriaEx(CurrentDate, yearCr, monthCr)
        '        _LabelValue = Me.Evaluate(CurrentDate, year, month).ToString
        '        If Me.HasBeweenFunctions Then
        '            _RangeCriteriaBetween = New BetweenOperator With {
        '            .BeginExpression = New ConstantValue(Me.StartFunction.Evaluate(year, month)),
        '            .EndExpression = New ConstantValue(Me.EndFunction.Evaluate(year, month))
        '        }
        '        End If
        '    ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedQuarter) Then
        '        _RangeCriteria = Me.ToCriteriaEx(CurrentDate, yearCr, quarterCr)
        '        _LabelValue = Me.Evaluate(CurrentDate, year, quarter).ToString
        '        If Me.HasBeweenFunctions Then
        '            _RangeCriteriaBetween = New BetweenOperator With {
        '            .BeginExpression = New ConstantValue(Me.StartFunction.Evaluate(year, quarter)),
        '            .EndExpression = New ConstantValue(Me.EndFunction.Evaluate(year, quarter))
        '        }
        '        End If
        '    ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedYear) Then
        '        _RangeCriteria = Me.ToCriteriaEx(CurrentDate, yearCr)
        '        _LabelValue = Me.Evaluate(CurrentDate, year).ToString
        '        If Me.HasBeweenFunctions Then
        '            _RangeCriteriaBetween = New BetweenOperator With {
        '            .BeginExpression = New ConstantValue(Me.StartFunction.Evaluate(year)),
        '            .EndExpression = New ConstantValue(Me.EndFunction.Evaluate(year))
        '        }
        '        End If
        '    End If
        'End Sub

    End Class
End Namespace

