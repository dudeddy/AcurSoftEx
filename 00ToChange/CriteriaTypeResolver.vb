Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers

Namespace AcurSoft.Data.Filtering.Helpers
    Public Class CriteriaTypeResolver
        Inherits CriteriaTypeResolverBase
        Implements IClientCriteriaVisitor(Of CriteriaTypeResolverResult)

        Private _PropertiesTypes As Dictionary(Of String, Type)
        Public Sub New(ByVal propertiesTypes As Dictionary(Of String, Type))
            MyBase.New()
            _PropertiesTypes = propertiesTypes
        End Sub
        Public Function Visit(ByVal theOperand As JoinOperand) As CriteriaTypeResolverResult Implements IClientCriteriaVisitor(Of CriteriaTypeResolverResult).Visit
            Throw New NotImplementedException()
        End Function
        Public Function Visit(ByVal theOperand As OperandProperty) As CriteriaTypeResolverResult Implements IClientCriteriaVisitor(Of CriteriaTypeResolverResult).Visit
            Dim result As Type = Nothing
            If Not _PropertiesTypes.TryGetValue(theOperand.PropertyName, result) Then
                Return New CriteriaTypeResolverResult(GetType(Object))
            End If
            Return New CriteriaTypeResolverResult(result)
        End Function
        Public Function Visit(ByVal theOperand As AggregateOperand) As CriteriaTypeResolverResult Implements IClientCriteriaVisitor(Of CriteriaTypeResolverResult).Visit
            Throw New NotImplementedException()
        End Function
        Public Function Resolve(ByVal criteria As CriteriaOperator) As Type
            Return Process(criteria).Type
        End Function

    End Class
End Namespace
