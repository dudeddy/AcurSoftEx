Imports DevExpress.Data.Filtering
Public Class GridFilterAccentSubstitutor
    Inherits ClientCriteriaVisitorBase

    Public Shared Function WrapIntoCustomFunction(ByVal param As CriteriaOperator) As CriteriaOperator
        Return New FunctionOperator(FunctionOperatorType.Custom, New ConstantValue(RemoveDiacriticsFunction.FunctionName), CType(param, CriteriaOperator))
    End Function

    Public Shared Function Substitute(ByVal source As CriteriaOperator) As CriteriaOperator
        Return New GridFilterAccentSubstitutor().AcceptOperator(source)
    End Function

    Protected Overrides Function VisitFunction(ByVal theOperator As FunctionOperator) As CriteriaOperator
        If theOperator.OperatorType = FunctionOperatorType.StartsWith OrElse theOperator.OperatorType = FunctionOperatorType.EndsWith OrElse theOperator.OperatorType = FunctionOperatorType.Contains Then
            Return New FunctionOperator(theOperator.OperatorType, GridFilterAccentSubstitutor.WrapIntoCustomFunction(theOperator.Operands(0)), GridFilterAccentSubstitutor.WrapIntoCustomFunction(theOperator.Operands(1)))
        End If
        Return MyBase.VisitFunction(theOperator)
    End Function
End Class