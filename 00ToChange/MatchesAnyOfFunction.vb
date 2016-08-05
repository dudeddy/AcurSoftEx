Imports DevExpress.Data.Filtering

Public NotInheritable Class MatchesAnyOfFunction
    Implements ICustomFunctionOperator

    Private Function ICustomFunctionOperator_Evaluate(ParamArray ByVal operands() As Object) As Object Implements ICustomFunctionOperator.Evaluate
        Throw New System.NotImplementedException()
    End Function

    Private ReadOnly Property ICustomFunctionOperator_Name() As String Implements ICustomFunctionOperator.Name
        Get
            Return "MatchesAnyOf"
        End Get
    End Property

    Private Function ICustomFunctionOperator_ResultType(ParamArray ByVal operands() As Type) As Type Implements ICustomFunctionOperator.ResultType
        Return GetType(Boolean)
    End Function
End Class
