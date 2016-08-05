Imports System.ComponentModel
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers

Namespace AcurSoft.Data.Filtering









    Public Class DateRangeCriteria
        Inherits DateCriteria
        Public ReadOnly Property RangeFunction As XpoFunctionForDateRanges
            Get
                Return DirectCast(Me.DateFunction, XpoFunctionForDateRanges)
            End Get
        End Property
        Public ReadOnly Property RangeCriteriaBetween As CriteriaOperator


        Public ReadOnly Property RangeCriteriaBetweenText As String
            Get
                If Me.RangeFunction Is Nothing Then Return ""
                If Me.RangeFunction.HasBeweenFunctions Then
                    Return Me.RangeCriteriaBetween.ToString
                End If
                Return ""
            End Get
        End Property
        Public Sub New(helper As DateChooseHelper, year As Integer, quarter As Integer, month As Integer, monthWeekNumber As Integer, fullWeek As Boolean, firstDayOfWeek As Integer)
            MyBase.New(helper, year, quarter, month, monthWeekNumber, fullWeek, firstDayOfWeek)
        End Sub


        Public Overrides Function UpdateRangeCriteria(criterias As List(Of CriteriaOperator), values As Object()) As DateCriteria
            MyBase.UpdateRangeCriteria(criterias, values)
            If RangeFunction.HasBeweenFunctions Then
                _RangeCriteriaBetween = New BetweenOperator With {
                    .TestExpression = Me.PropertyCriteria,
                    .BeginExpression = New ConstantValue(RangeFunction.StartFunction.Evaluate(values)),
                    .EndExpression = New ConstantValue(RangeFunction.EndFunction.Evaluate(values))
                }
            End If
            Return Me
        End Function
    End Class


End Namespace
