
Imports DevExpress.Data.Filtering


Namespace AcurSoft.Data.Filtering

    Public Class DateCriteria
        Public Property Year As DateCriteriaElement
        Public Property Month As DateCriteriaElement
        Public Property Quarter As DateCriteriaElement

        Public Property MonthWeekNumber As DateCriteriaElementBase(Of Integer)

        Public Property FullWeek As DateCriteriaElementBase(Of Boolean)

        Public Property FirstDayOfWeek As DateCriteriaElementBase(Of Integer)

        Public Property Helper As DateChooseHelper

        Public ReadOnly Property DateFunction As XpoFunctionForDates
        Public ReadOnly Property LabelValue As String
        Public ReadOnly Property Criteria As CriteriaOperator
        Public Property PropertyCriteria As OperandProperty


        Public Sub New(helper As DateChooseHelper, year As Integer, quarter As Integer, month As Integer, monthWeekNumber As Integer, fullWeek As Boolean, firstDayOfWeek As Integer)
            Me.Helper = helper
            Me.MonthWeekNumber = New DateCriteriaElementBase(Of Integer)(monthWeekNumber)
            Me.FirstDayOfWeek = New DateCriteriaElementBase(Of Integer)(firstDayOfWeek)
            Me.FullWeek = New DateCriteriaElementBase(Of Boolean)(fullWeek)

            Me.Year = New DateCriteriaElement(year) With {.ThisCriteria = "ThisYear()"}
            Me.Month = New DateCriteriaElement(month) With {.ThisCriteria = "ThisMonth()"}
            Me.Quarter = New DateCriteriaElement(quarter) With {.ThisCriteria = "ThisQuarter()"}

            Select Case helper.DateChooseKind
                Case DateChooseKind.Choose, DateChooseKind.None
                Case Else
                    Select Case helper.DateStartKind
                        Case DateStartKind.Year
                            Me.Year.DateChooseKind = helper.DateChooseKind
                            Me.Year.AfterAgoValue = helper.AfterAgoValue
                        Case DateStartKind.Month
                            Me.Month.DateChooseKind = helper.DateChooseKind
                            Me.Month.AfterAgoValue = helper.AfterAgoValue
                        Case DateStartKind.Quarter
                            Me.Quarter.DateChooseKind = helper.DateChooseKind
                            Me.Quarter.AfterAgoValue = helper.AfterAgoValue
                    End Select
            End Select
        End Sub
        'Public ReadOnly Property RangeCriteriaBetweenText As String
        '    Get
        '        If Me.RangeFunction Is Nothing Then Return ""
        '        If Me.RangeFunction.HasBeweenFunctions Then
        '            Return Me.RangeCriteriaBetween.ToString
        '        End If
        '        Return ""
        '    End Get
        'End Property

        Public Overridable Function UpdateRangeCriteria(criterias As List(Of CriteriaOperator), values As Object()) As DateCriteria
            _Criteria = DateFunction.ToCriteriaEx(criterias.ToArray)
            'If RangeFunction.HasBeweenFunctions Then
            '    _RangeCriteriaBetween = New BetweenOperator With {
            '        .TestExpression = _PropertyCriteria,
            '        .BeginExpression = New ConstantValue(RangeFunction.StartFunction.Evaluate(values)),
            '        .EndExpression = New ConstantValue(RangeFunction.EndFunction.Evaluate(values))
            '    }
            'End If
            Return Me
        End Function
        Public Function UpdateRangeCriteria(dateFunction As XpoFunctionForDates) As DateCriteria
            _DateFunction = dateFunction
            Dim h As New DateChooseHelper(dateFunction.Kind)
            Dim criterias As List(Of CriteriaOperator) = {_PropertyCriteria, Me.Year.Criteria, Me.Month.Criteria, Me.MonthWeekNumber.Criteria, Me.FullWeek.Criteria, Me.FirstDayOfWeek.Criteria}.ToList()
            'Dim values As Object() = {CurrentDate, Me.Year.Value, Me.Month.Value, Me.MonthWeekNumber.Value, Me.FullWeek.Value, Me.FirstDayOfWeek.Value}
            Dim values As Object() = {Me.Year.Value, Me.Month.Value, Me.MonthWeekNumber.Value, Me.FullWeek.Value, Me.FirstDayOfWeek.Value}


            If dateFunction.Kind.HasFlag(DateFunctionKind.NeedMonthWeekNumber) Then
            ElseIf h.DateStartKind = DateStartKind.Month Then
                criterias = {_PropertyCriteria, Me.Year.Criteria, Me.Month.Criteria}.ToList()
                values = {Me.Year.Value, Me.Month.Value}
            ElseIf h.DateStartKind = DateStartKind.Quarter Then
                criterias = {_PropertyCriteria, Me.Year.Criteria, Me.Quarter.Criteria}.ToList()
                values = {Me.Year.Value, Me.Quarter.Value}
            ElseIf h.DateStartKind = DateStartKind.Year Then
                criterias = {_PropertyCriteria, Me.Year.Criteria}.ToList()
                values = {Me.Year.Value}
            End If
            Return Me.UpdateRangeCriteria(criterias, values)

        End Function

        Public Function UpdateRangeCriteria(dateFunction As XpoFunctionForDates, propertyCriteria As OperandProperty) As DateCriteria
            _PropertyCriteria = propertyCriteria
            Return Me.UpdateRangeCriteria(dateFunction)
        End Function
        Public Function UpdateRangeCriteria(dateFunction As XpoFunctionForDates, propertyName As String) As DateCriteria
            _PropertyCriteria = New OperandProperty(propertyName)
            Return Me.UpdateRangeCriteria(dateFunction)
        End Function

    End Class


End Namespace
