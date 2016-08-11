Imports System.ComponentModel
Imports DevExpress.Data.Filtering

Namespace AcurSoft.Data.Filtering.Summary

    Public Class SummaryExpressionCriteriaVisitor
        Implements IClientCriteriaVisitor(Of CriteriaOperator)

        Public Sub New()
        End Sub
        Public ReadOnly Property Criteria As CriteriaOperator

        Private _PropertyDescriptors As PropertyDescriptorCollection
        Public Sub New(ByVal propertyDescriptors As PropertyDescriptorCollection, ByVal op As CriteriaOperator)
            Me.Criteria = op
            _PropertyDescriptors = propertyDescriptors
        End Sub
        Public Sub New(ByVal op As CriteriaOperator)
            Me.Criteria = op
        End Sub

        Public Function Fix() As CriteriaOperator
            Return Me.Criteria.Accept(Of CriteriaOperator)(Me)
        End Function


        Public Shared Function Fix(ByVal op As CriteriaOperator) As CriteriaOperator
            Return op.Accept(Of CriteriaOperator)(New SummaryExpressionCriteriaVisitor(op))
        End Function

        Public Shared Function Fix(ByVal op As CriteriaOperator, propertyDescriptors As PropertyDescriptorCollection) As CriteriaOperator
            Return op.Accept(Of CriteriaOperator)(New SummaryExpressionCriteriaVisitor(propertyDescriptors, op))
        End Function


#Region "IClientCriteriaVisitor Members"

        Public Function Visit(ByVal theOperand As JoinOperand) As CriteriaOperator Implements IClientCriteriaVisitor(Of CriteriaOperator).Visit
            Dim condition As CriteriaOperator = TryCast(theOperand.Condition?.Accept(Me), CriteriaOperator)
            Dim expression As CriteriaOperator = TryCast(theOperand.AggregatedExpression?.Accept(Me), CriteriaOperator)
            If condition Is Nothing OrElse expression Is Nothing Then Return Nothing
            Return New JoinOperand(theOperand.JoinTypeName, condition, theOperand.AggregateType, expression)
        End Function

        Public Function Visit(ByVal theOperand As OperandProperty) As CriteriaOperator Implements IClientCriteriaVisitor(Of CriteriaOperator).Visit
            If _PropertyDescriptors IsNot Nothing Then
                Dim dp As PropertyDescriptor = _PropertyDescriptors.Find(theOperand.PropertyName, True)
                If dp IsNot Nothing Then
                    Return New OperandProperty(dp.Name)
                End If
            End If
            Return theOperand
        End Function

        Public Function Visit(ByVal theOperand As AggregateOperand) As CriteriaOperator Implements IClientCriteriaVisitor(Of CriteriaOperator).Visit
            Dim operand As OperandProperty = TryCast(theOperand.CollectionProperty?.Accept(Me), OperandProperty)
            Dim condition As CriteriaOperator = TryCast(theOperand.Condition?.Accept(Me), CriteriaOperator)
            If operand Is Nothing Then
                operand = New OperandProperty("this")
            ElseIf condition Is Nothing AndAlso Not (TypeOf operand Is OperandProperty AndAlso DirectCast(operand, OperandProperty).PropertyName.ToUpper = "THIS") Then
                condition = CriteriaOperator.Parse(operand.ToString)
                operand = New OperandProperty("this")
            End If

            Dim expression As CriteriaOperator = TryCast(theOperand.AggregatedExpression?.Accept(Me), CriteriaOperator)
            If operand Is Nothing Then Return Nothing
            Return New AggregateOperand(operand, expression, theOperand.AggregateType, condition)
        End Function
#End Region

#Region "ICriteriaVisitor Members"

        Public Function Visit(ByVal theOperator As FunctionOperator) As CriteriaOperator Implements ICriteriaVisitor(Of CriteriaOperator).Visit
            If theOperator.OperatorType = FunctionOperatorType.Custom Then
                If SummaryExpressionFunction.IsExpressionFunction(DirectCast(theOperator.Operands(0), ConstantValue).Value.ToString()) Then
                    'If {"RANK", "SUMTOP", "SUMBOTTOM", "AVGTOP", "AVGBOTTOM"}.Contains(DirectCast(theOperator.Operands(0), ConstantValue).Value.ToString().ToUpper) Then
                    If TypeOf theOperator.Operands(1) Is OperandProperty Then
                        theOperator.Operands(1) = New ConstantValue(DirectCast(theOperator.Operands(1), OperandProperty).PropertyName)
                    ElseIf TypeOf theOperator.Operands(1) IsNot ConstantValue Then
                        theOperator.Operands(1) = New ConstantValue(theOperator.Operands(1).ToString)
                    End If
                End If
            End If
            Dim operators As New List(Of CriteriaOperator)()
            For Each op As CriteriaOperator In theOperator.Operands
                Dim temp As CriteriaOperator = op.Accept(Of CriteriaOperator)(Me)
                If temp Is Nothing Then Return Nothing
                operators.Add(temp)
            Next op
            Return New FunctionOperator(theOperator.OperatorType, operators)
        End Function

        Public Function Visit(ByVal theOperand As OperandValue) As CriteriaOperator Implements ICriteriaVisitor(Of CriteriaOperator).Visit
            Return theOperand
        End Function

        Public Function Visit(ByVal theOperator As GroupOperator) As CriteriaOperator Implements ICriteriaVisitor(Of CriteriaOperator).Visit
            Dim operators As New List(Of CriteriaOperator)()
            For Each op As CriteriaOperator In theOperator.Operands
                Dim temp As CriteriaOperator = op.Accept(Of CriteriaOperator)(Me)
                If temp Is Nothing Then Continue For
                operators.Add(temp)
            Next op
            Return New GroupOperator(theOperator.OperatorType, operators)
        End Function

        Public Function Visit(ByVal theOperator As InOperator) As CriteriaOperator Implements ICriteriaVisitor(Of CriteriaOperator).Visit
            Dim leftOperand As CriteriaOperator = theOperator.LeftOperand.Accept(Of CriteriaOperator)(Me)
            If leftOperand Is Nothing Then Return Nothing
            Dim operators As New List(Of CriteriaOperator)()
            For Each op As CriteriaOperator In theOperator.Operands
                Dim temp As CriteriaOperator = op.Accept(Of CriteriaOperator)(Me)
                If temp Is Nothing Then Continue For
                operators.Add(temp)
            Next op
            Return New InOperator(leftOperand, operators)
        End Function

        Public Function Visit(ByVal theOperator As UnaryOperator) As CriteriaOperator Implements ICriteriaVisitor(Of CriteriaOperator).Visit
            Dim operand As CriteriaOperator = theOperator.Operand.Accept(Of CriteriaOperator)(Me)
            If operand Is Nothing Then Return Nothing
            Return New UnaryOperator(theOperator.OperatorType, operand)
        End Function

        Public Function Visit(ByVal theOperator As BinaryOperator) As CriteriaOperator Implements ICriteriaVisitor(Of CriteriaOperator).Visit
            Dim leftOperand As CriteriaOperator = theOperator.LeftOperand.Accept(Of CriteriaOperator)(Me)
            Dim rightOperand As CriteriaOperator = theOperator.RightOperand.Accept(Of CriteriaOperator)(Me)
            If leftOperand Is Nothing OrElse rightOperand Is Nothing Then Return Nothing
            Return New BinaryOperator(leftOperand, rightOperand, theOperator.OperatorType)
        End Function

        Public Function Visit(ByVal theOperator As BetweenOperator) As CriteriaOperator Implements ICriteriaVisitor(Of CriteriaOperator).Visit
            Dim test As CriteriaOperator = theOperator.TestExpression.Accept(Of CriteriaOperator)(Me)
            Dim begin As CriteriaOperator = theOperator.BeginExpression.Accept(Of CriteriaOperator)(Me)
            Dim [end] As CriteriaOperator = theOperator.EndExpression.Accept(Of CriteriaOperator)(Me)
            If test Is Nothing OrElse begin Is Nothing OrElse [end] Is Nothing Then Return Nothing
            Return New BetweenOperator(test, begin, [end])
        End Function
#End Region

    End Class
End Namespace
