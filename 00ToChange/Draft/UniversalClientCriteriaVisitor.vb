Imports DevExpress.Data.Filtering

Namespace AcurSoft.Data.Filtering.Helpers

    Public Enum OperandKing
        None
        Value
        JoinAggregatedExpression
        JoinCondition
        BinaryLeft
        BinaryRight
        InLeft
        InOperands
        FunctionOperands
        GroupOperands
        UnaryOperand
        BetweenBeginExpression
        BetweenEndExpression
        BetweenTestExpression
        [Property]
        AggregateAggregatedExpression
        AggregateCondition
        AggregateCollectionProperty
    End Enum

    Public Class UniversalClientCriteriaVisitor(Of T)
        Implements IClientCriteriaVisitor(Of T), ICriteriaVisitor(Of T)

        Public Event OnVisiting(e As UniversalClientCriteriaVisitingArg)
        Public Class UniversalClientCriteriaVisitingArg
            Public ReadOnly Property Kind As CriteriaKind
                Get
                    Return Me.Criteria.GetCritriaKind()
                End Get
            End Property


            Public Property Criteria As CriteriaOperator
            'Public Property NewCriteria As CriteriaOperator
            Public Property Handled As Boolean
            Public Property ChildrenHandled As Boolean
            Public Property NewObject As T
            Public Property ProcessChildren As Action
            Public Property AfterAction As Action(Of CriteriaOperator, T)

            'Private _Children As List(Of T)
            'Public ReadOnly Property Children As List(Of T)
            '    Get
            '        If _Children Is Nothing Then
            '            _Children = New List(Of T)
            '        End If
            '        Return _Children
            '    End Get
            'End Property

            Private _Infos As List(Of VisiteInfo)
            Public ReadOnly Property Infos As List(Of VisiteInfo)
                Get
                    If _Infos Is Nothing Then
                        _Infos = New List(Of VisiteInfo)
                    End If
                    Return _Infos
                End Get
            End Property
            Public Property CreateNew As Func(Of T)
            Public Function GetValue() As T
                If Not Me.ChildrenHandled AndAlso Me.ProcessChildren IsNot Nothing Then
                    Me.ProcessChildren.Invoke()
                End If

                If Me.CreateNew IsNot Nothing Then
                    Me.NewObject = Me.CreateNew.Invoke()
                End If

                If Me.AfterAction IsNot Nothing Then
                    Me.AfterAction.Invoke(Me.Criteria, Me.NewObject)
                End If
                Return Me.NewObject

            End Function


            Public Function Execute() As T
                If Not Me.Handled Then
                    Return Me.GetValue()
                End If
                Return Me.NewObject
            End Function
        End Class

        Public Class VisiteInfo
            Public Property Parent As T
            Public Property ThisObject As T
            'Public ReadOnly Property ParentObject As T
            '    Get
            '        If Me.Parent Is Nothing Then Return Nothing
            '        Return Me.Parent.ThisObject
            '    End Get
            'End Property

            Public Property Level As Integer
            Public ReadOnly Property ParentKind As CriteriaKind
            Public ReadOnly Property Kind As CriteriaKind
            Public ReadOnly Property OperandKing As OperandKing
            Public ReadOnly Property OperandIndex As Integer
            Public ReadOnly Property Criteria As CriteriaOperator
            Public ReadOnly Property ParentCriteria As CriteriaOperator
            Public ReadOnly Property IsSelf As Boolean
#Region "Construct"

            Public Sub New(criteria As CriteriaOperator, parentCriteria As CriteriaOperator, operandIndex As Integer, operandKing As OperandKing)
                Me.New(criteria, parentCriteria, operandIndex, operandKing, Nothing, Nothing)
            End Sub
            Public Sub New(criteria As CriteriaOperator, parentCriteria As CriteriaOperator, operandKing As OperandKing)
                Me.New(criteria, parentCriteria, -1, operandKing, Nothing, Nothing)
            End Sub
            Public Sub New(criteria As CriteriaOperator, parentCriteria As CriteriaOperator, operandIndex As Integer, operandKing As OperandKing, thisObject As T)
                Me.New(criteria, parentCriteria, operandIndex, operandKing, thisObject, Nothing)
            End Sub
            Public Sub New(criteria As CriteriaOperator, parentCriteria As CriteriaOperator, operandKing As OperandKing, thisObject As T)
                Me.New(criteria, parentCriteria, -1, operandKing, thisObject, Nothing)
            End Sub
            Public Sub New(criteria As CriteriaOperator, thisObject As T)
                Me.New(criteria, Nothing, -1, OperandKing.None, thisObject, Nothing)
            End Sub
            Public Sub New(criteria As CriteriaOperator, parentCriteria As CriteriaOperator,
                           operandIndex As Integer, operandKing As OperandKing,
                           thisObject As T,
                           parent As T)
                Me.Criteria = criteria
                Me.ParentCriteria = parentCriteria
                Me.Kind = Me.Criteria.GetCritriaKind()
                Me.ParentKind = Me.ParentCriteria.GetCritriaKind()
                Me.IsSelf = Me.Kind = CriteriaKind.OperandValue OrElse Me.Kind = CriteriaKind.OperandProperty
                Me.OperandIndex = operandIndex
                Me.OperandKing = operandKing
                Me.ThisObject = thisObject
                Me.Parent = parent
            End Sub

#End Region

        End Class

        Public ReadOnly Property Factory As Func(Of CriteriaOperator, T)
        Public ReadOnly Property Criteria As CriteriaOperator

        Public Sub New(cr As CriteriaOperator, factory As Func(Of CriteriaOperator, T))
            _Criteria = cr
            _Factory = factory
        End Sub
        Public Function Process(criteria As CriteriaOperator) As T
            Return criteria.Accept(Me)
        End Function


        Public Function Visit(theOperand As JoinOperand) As T Implements IClientCriteriaVisitor(Of T).Visit
            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                .Criteria = theOperand,
                .CreateNew = Function() Factory(theOperand)}
            arg.ProcessChildren =
                Sub()
                    If theOperand.AggregatedExpression IsNot Nothing Then
                        arg.Infos.Add(New VisiteInfo(theOperand.AggregatedExpression, theOperand, OperandKing.JoinAggregatedExpression, theOperand.AggregatedExpression.Accept(Me)))
                    End If
                    If theOperand.Condition IsNot Nothing Then
                        arg.Infos.Add(New VisiteInfo(theOperand.Condition, theOperand, OperandKing.JoinCondition, theOperand.Condition.Accept(Me)))
                    End If
                End Sub
            RaiseEvent OnVisiting(arg)
            Return arg.Execute
        End Function

        Public Function Visit(theOperator As BinaryOperator) As T Implements IClientCriteriaVisitor(Of T).Visit
            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                .Criteria = theOperator,
                .CreateNew = Function() Factory(theOperator)}
            arg.ProcessChildren =
                Sub()
                    arg.Infos.Add(New VisiteInfo(theOperator.LeftOperand, theOperator, OperandKing.BinaryLeft, theOperator.LeftOperand.Accept(Me)))
                    arg.Infos.Add(New VisiteInfo(theOperator.RightOperand, theOperator, OperandKing.BinaryRight, theOperator.RightOperand.Accept(Me)))
                End Sub

            RaiseEvent OnVisiting(arg)
            Dim p As T = arg.Execute
            Dim vp As New VisiteInfo(theOperator, p)

            For Each c In arg.Infos
                c.Parent = p
            Next
            Return p
        End Function

        Public Function Visit(theOperator As InOperator) As T Implements IClientCriteriaVisitor(Of T).Visit
            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                .Criteria = theOperator,
                .CreateNew = Function() Factory(theOperator)}
            arg.ProcessChildren =
                Sub()
                    arg.Infos.Add(New VisiteInfo(theOperator.LeftOperand, theOperator, OperandKing.InLeft, theOperator.LeftOperand.Accept(Me)))

                    Dim i As Integer = 0
                    For Each o In theOperator.Operands
                        arg.Infos.Add(New VisiteInfo(o, theOperator, i, OperandKing.InOperands, o.Accept(Me)))
                        i += 1
                    Next
                End Sub

            RaiseEvent OnVisiting(arg)
            Dim p As T = arg.Execute
            Dim vp As New VisiteInfo(theOperator, p)

            For Each c In arg.Infos
                c.Parent = p
            Next
            Return p
        End Function

        Public Function Visit(theOperand As OperandValue) As T Implements IClientCriteriaVisitor(Of T).Visit
            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                           .Criteria = theOperand,
                           .CreateNew = Function() Factory(theOperand)}
            RaiseEvent OnVisiting(arg)
            arg.Infos.Add(New VisiteInfo(theOperand, theOperand, OperandKing.Value, theOperand.Accept(Me)))

            Return arg.Execute
            'Throw New NotImplementedException()
        End Function

        Public Function Visit(theOperator As FunctionOperator) As T Implements IClientCriteriaVisitor(Of T).Visit
            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                .Criteria = theOperator,
                .CreateNew = Function() Factory(theOperator)}
            arg.ProcessChildren =
                Sub()
                    Dim i As Integer = 0
                    For Each o In theOperator.Operands
                        arg.Infos.Add(New VisiteInfo(o, theOperator, i, OperandKing.FunctionOperands, o.Accept(Me)))

                        i += 1
                    Next
                End Sub

            RaiseEvent OnVisiting(arg)
            Return arg.Execute
        End Function

        Public Function Visit(theOperator As GroupOperator) As T Implements IClientCriteriaVisitor(Of T).Visit
            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                .Criteria = theOperator,
                .CreateNew = Function() Factory(theOperator)}
            arg.ProcessChildren =
                Sub()
                    Dim i As Integer = 0
                    For Each o In theOperator.Operands
                        arg.Infos.Add(New VisiteInfo(o, theOperator, i, OperandKing.GroupOperands, o.Accept(Me)))
                        i += 1
                    Next
                End Sub

            RaiseEvent OnVisiting(arg)
            Return arg.Execute
        End Function

        Public Function Visit(theOperator As UnaryOperator) As T Implements IClientCriteriaVisitor(Of T).Visit
            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                .Criteria = theOperator,
                .CreateNew = Function() Factory(theOperator)}
            arg.ProcessChildren =
                Sub()
                    arg.Infos.Add(New VisiteInfo(theOperator.Operand, theOperator, OperandKing.UnaryOperand, theOperator.Operand.Accept(Me)))
                End Sub

            RaiseEvent OnVisiting(arg)
            Return arg.Execute
        End Function

        Public Function Visit(theOperator As BetweenOperator) As T Implements IClientCriteriaVisitor(Of T).Visit
            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                .Criteria = theOperator,
                .CreateNew = Function() Factory(theOperator)}
            arg.ProcessChildren =
                Sub()
                    arg.Infos.Add(New VisiteInfo(theOperator.BeginExpression, theOperator, OperandKing.BetweenBeginExpression, theOperator.BeginExpression.Accept(Me)))
                    arg.Infos.Add(New VisiteInfo(theOperator.EndExpression, theOperator, OperandKing.BetweenEndExpression, theOperator.EndExpression.Accept(Me)))
                    arg.Infos.Add(New VisiteInfo(theOperator.TestExpression, theOperator, OperandKing.BetweenTestExpression, theOperator.TestExpression.Accept(Me)))
                End Sub
            RaiseEvent OnVisiting(arg)
            Return arg.Execute
        End Function

        Public Function Visit(theOperand As OperandProperty) As T Implements IClientCriteriaVisitor(Of T).Visit
            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                           .Criteria = theOperand,
                           .CreateNew = Function() Factory(theOperand)}
            RaiseEvent OnVisiting(arg)
            arg.Infos.Add(New VisiteInfo(theOperand, theOperand, OperandKing.Property, theOperand.Accept(Me)))
            Return arg.Execute
            'Throw New NotImplementedException()
        End Function

        Public Function Visit(theOperand As AggregateOperand) As T Implements IClientCriteriaVisitor(Of T).Visit

            Dim arg As New UniversalClientCriteriaVisitingArg() With {
                .Criteria = theOperand,
                .CreateNew = Function() Factory(theOperand)}
            arg.ProcessChildren =
                Sub()
                    If theOperand.CollectionProperty IsNot Nothing Then
                        arg.Infos.Add(New VisiteInfo(theOperand.CollectionProperty, theOperand, OperandKing.AggregateCollectionProperty, theOperand.CollectionProperty.Accept(Me)))
                    End If
                    If theOperand.AggregatedExpression IsNot Nothing Then
                        arg.Infos.Add(New VisiteInfo(theOperand.AggregatedExpression, theOperand, OperandKing.AggregateAggregatedExpression, theOperand.AggregatedExpression.Accept(Me)))
                    End If
                    If theOperand.Condition IsNot Nothing Then
                        arg.Infos.Add(New VisiteInfo(theOperand.Condition, theOperand, OperandKing.AggregateCondition, theOperand.Condition.Accept(Me)))
                    End If

                End Sub
            RaiseEvent OnVisiting(arg)
            Return arg.Execute
        End Function

    End Class
End Namespace
