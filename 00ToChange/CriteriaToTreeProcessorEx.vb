Imports System.Reflection
Imports AcurSoft.Data.Filtering.Helpers
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.Data.Filtering.Helpers
    Public Class CriteriaToTreeProcessorEx
        Inherits CriteriaToTreeProcessor
        Implements IClientCriteriaVisitor(Of INode), ICriteriaVisitor(Of INode)
        Public ReadOnly Property NodesFactory As FilterControlNodesFactory
        Public Sub New(ByVal nodesFactory As INodesFactory, ByVal skippedHolder As IList(Of CriteriaOperator))
            MyBase.New(nodesFactory, skippedHolder)
            _NodesFactory = DirectCast(nodesFactory, FilterControlNodesFactory)
        End Sub

        ''Function Visit(theOperator As UnaryOperator) As T
        'Function Visit(theOperand As OperandValue) As T
        ''Function Visit(theOperator As BetweenOperator) As T
        ''Function Visit(theOperator As BinaryOperator) As T
        ''Function Visit(theOperator As InOperator) As T
        ''Function Visit(theOperator As GroupOperator) As T
        ''Function Visit(theOperator As FunctionOperator) As T

        'Function Visit(theOperand As AggregateOperand) As T
        'Function Visit(theOperand As OperandProperty) As T
        'Function Visit(theOperand As JoinOperand) As T

        Private Function GetInvertedOperation(ByVal subNode As IClauseNode) As ClauseType
            Dim aggregateNode As IAggregateNode = TryCast(subNode, IAggregateNode)
            If aggregateNode IsNot Nothing AndAlso aggregateNode.Aggregate = Aggregate.Exists Then
                Return subNode.Operation
            End If
            Select Case subNode.Operation
                Case ClauseType.AnyOf
                    Return ClauseType.NoneOf
                Case ClauseType.Between
                    Return ClauseType.NotBetween
                Case ClauseType.Contains
                    Return ClauseType.DoesNotContain
                Case ClauseType.Equals
                    Return ClauseType.DoesNotEqual
                Case ClauseType.Greater
                    Return ClauseType.LessOrEqual
                Case ClauseType.GreaterOrEqual
                    Return ClauseType.Less
                Case ClauseType.Like
                    Return ClauseType.NotLike
                Case ClauseType.IsNotNull
                    Return ClauseType.IsNull
                Case ClauseType.IsNull
                    Return ClauseType.IsNotNull
                Case ClauseType.IsNullOrEmpty
                    Return ClauseType.IsNotNullOrEmpty
                Case ClauseType.IsNotNullOrEmpty
                    Return ClauseType.IsNullOrEmpty
                Case ClauseType.Less
                    Return ClauseType.GreaterOrEqual
                Case ClauseType.LessOrEqual
                    Return ClauseType.Greater
                Case ClauseType.NoneOf
                    Return ClauseType.AnyOf
                Case ClauseType.NotBetween
                    Return ClauseType.Between
                Case ClauseType.DoesNotContain
                    Return ClauseType.Contains
                Case ClauseType.DoesNotEqual
                    Return ClauseType.Equals
                Case ClauseType.NotLike
                    Return ClauseType.Like
            End Select
            Return subNode.Operation
        End Function


        Private Function IClientCriteriaVisitorGeneric_Visit(ByVal theOperand As AggregateOperand) As INode Implements IClientCriteriaVisitor(Of INode).Visit
            'If FactoryEx Is Nothing Then
            '    Return Skip(theOperand)
            'End If
            Dim condition As INode = CriteriaToTreeProcessor.GetTree(Factory, theOperand.Condition, Skipped)
            Return Me.NodesFactory.Create(theOperand.CollectionProperty, theOperand.AggregateType, TryCast(theOperand.AggregatedExpression, OperandProperty), ClauseType.IsNull, Nothing, condition)
        End Function

        Private Function ICriteriaVisitorGeneric_Visit(ByVal theOperator As UnaryOperator) As INode Implements ICriteriaVisitor(Of INode).Visit
            If theOperator.OperatorType = UnaryOperatorType.IsNull Then
                Return CreateClauseNode(theOperator, ClauseType.IsNull, theOperator.Operand, New CriteriaOperator() {})
            ElseIf theOperator.OperatorType = UnaryOperatorType.Not Then
                Dim subNode As INode = Process(theOperator.Operand)
                If TypeOf subNode Is IGroupNode Then
                    Dim gr As IGroupNode = DirectCast(subNode, IGroupNode)
                    Dim invertedType As GroupType
                    Select Case gr.NodeType
                        Case GroupType.And
                            invertedType = GroupType.NotAnd
                        Case GroupType.Or
                            invertedType = GroupType.NotOr
                        Case GroupType.NotAnd
                            invertedType = GroupType.And
                        Case GroupType.NotOr
                            invertedType = GroupType.Or
                        Case Else
                            Throw New NotImplementedException(gr.NodeType.ToString())
                    End Select
                    Return Factory.Create(invertedType, gr.SubNodes)
                ElseIf TypeOf subNode Is IClauseNode Then
                    Dim oldClause As IClauseNode = DirectCast(subNode, IClauseNode)
                    Dim invertedType As ClauseType = GetInvertedOperation(oldClause)
                    If invertedType <> oldClause.Operation Then
                        If TypeOf oldClause Is IAggregateNode Then
                            Dim oldAggregateNode As IAggregateNode = TryCast(subNode, IAggregateNode)
                            Return FactoryEx.Create(oldAggregateNode.FirstOperand, oldAggregateNode.Aggregate, oldAggregateNode.AggregateOperand, invertedType, oldAggregateNode.AdditionalOperands, oldAggregateNode.AggregateCondition)
                        Else
                            Return Factory.Create(invertedType, oldClause.FirstOperand, oldClause.AdditionalOperands)
                        End If
                    Else
                        Return Factory.Create(GroupType.NotAnd, New INode() {oldClause})
                    End If
                Else
                    Return Skip(theOperator)
                End If
            Else
                Return Skip(theOperator)
            End If
        End Function



        Private Function ICriteriaVisitorGeneric_Visit(ByVal theOperator As GroupOperator) As INode Implements ICriteriaVisitor(Of INode).Visit
            Dim resolvedType As GroupType = If(theOperator.OperatorType = GroupOperatorType.And, GroupType.And, GroupType.Or)
            Dim subNodes As New List(Of INode)()
            For Each subOperand As CriteriaOperator In theOperator.Operands
                Dim nestedNode As INode = Process(subOperand)
                If nestedNode IsNot Nothing Then
                    subNodes.Add(nestedNode)
                End If
            Next subOperand
            Return Me.NodesFactory.Create(resolvedType, subNodes)
        End Function



        Private Function ICriteriaVisitorGeneric_Visit(ByVal theOperator As FunctionOperator) As INode Implements ICriteriaVisitor(Of INode).Visit
            Select Case theOperator.OperatorType
                Case FunctionOperatorType.Custom
                    If LikeCustomFunction.IsBinaryCompatibleLikeFunction(theOperator) AndAlso Me.IsGoodForAdditionalOperands(theOperator.Operands(2)) Then
                        Return Me.CreateClauseNode(theOperator, ClauseType.Like, theOperator.Operands(1), New CriteriaOperator() {theOperator.Operands(2)})
                    Else
                        Return Me.VisitCustomFunction(theOperator)
                    End If

                Case FunctionOperatorType.IsNullOrEmpty
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsNullOrEmpty)

                Case FunctionOperatorType.StartsWith, FunctionOperatorType.EndsWith, FunctionOperatorType.Contains
                    Return Me.DoStartsEndsContains(theOperator)

                Case FunctionOperatorType.IsOutlookIntervalBeyondThisYear
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsBeyondThisYear)

                Case FunctionOperatorType.IsOutlookIntervalLaterThisYear
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsLaterThisYear)

                Case FunctionOperatorType.IsOutlookIntervalLaterThisMonth
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsLaterThisMonth)

                Case FunctionOperatorType.IsOutlookIntervalNextWeek
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsNextWeek)

                Case FunctionOperatorType.IsOutlookIntervalLaterThisWeek
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsLaterThisWeek)

                Case FunctionOperatorType.IsOutlookIntervalTomorrow
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsTomorrow)

                Case FunctionOperatorType.IsOutlookIntervalToday
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsToday)

                Case FunctionOperatorType.IsOutlookIntervalYesterday
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsYesterday)

                Case FunctionOperatorType.IsOutlookIntervalEarlierThisWeek
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsEarlierThisWeek)

                Case FunctionOperatorType.IsOutlookIntervalLastWeek
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsLastWeek)

                Case FunctionOperatorType.IsOutlookIntervalEarlierThisMonth
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsEarlierThisMonth)

                Case FunctionOperatorType.IsOutlookIntervalEarlierThisYear
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsEarlierThisYear)

                Case FunctionOperatorType.IsOutlookIntervalPriorThisYear
                    Return Me.CreateNodeForUnaryClause(theOperator, ClauseType.IsPriorThisYear)
            End Select
            'If TypeOf theOperator Is FunctionOperator Then
            '    Return Me.VisitCustomFunction(theOperator)
            'End If
            Return Me.Skip(theOperator)
        End Function

        Private Function ICriteriaVisitorGeneric_Visit(ByVal theOperator As BinaryOperator) As INode Implements ICriteriaVisitor(Of INode).Visit
            Dim type As ClauseType
            Select Case theOperator.OperatorType
                Case BinaryOperatorType.Equal
                    type = ClauseType.Equals
                Case BinaryOperatorType.Greater
                    type = ClauseType.Greater
                Case BinaryOperatorType.GreaterOrEqual
                    type = ClauseType.GreaterOrEqual
                Case BinaryOperatorType.Less
                    type = ClauseType.Less
                Case BinaryOperatorType.LessOrEqual
                    type = ClauseType.LessOrEqual
                'Case BinaryOperatorType.Like
                '    type = ClauseType.Like
                Case BinaryOperatorType.NotEqual
                    type = ClauseType.DoesNotEqual
                Case Else
                    Return Skip(theOperator)
            End Select
            If Not IsGoodForAdditionalOperands(theOperator.RightOperand) Then
                Return Skip(theOperator)
            End If
            Return CreateClauseNode(theOperator, type, theOperator.LeftOperand, New CriteriaOperator() {theOperator.RightOperand})
        End Function

        Private Function ICriteriaVisitorGeneric_Visit(ByVal theOperator As InOperator) As INode Implements ICriteriaVisitor(Of INode).Visit
            Dim operands As New List(Of CriteriaOperator)()
            For Each ao As CriteriaOperator In theOperator.Operands
                If IsGoodForAdditionalOperands(ao) Then
                    operands.Add(ao)
                Else
                    Return Skip(theOperator)
                End If
            Next ao
            Return CreateClauseNode(theOperator, ClauseType.AnyOf, theOperator.LeftOperand, operands)
        End Function
        Private Function ICriteriaVisitorGeneric_Visit(ByVal theOperator As BetweenOperator) As INode Implements ICriteriaVisitor(Of INode).Visit
            If Not IsGoodForAdditionalOperands(theOperator.BeginExpression) Then
                Return Skip(theOperator)
            End If
            If Not IsGoodForAdditionalOperands(theOperator.EndExpression) Then
                Return Skip(theOperator)
            End If
            Return CreateClauseNode(theOperator, ClauseType.Between, theOperator.TestExpression, New CriteriaOperator() {theOperator.BeginExpression, theOperator.EndExpression})
        End Function


        Protected Function CreateClauseNodeEx(ByVal origionalOperator As CriteriaOperator, ByVal type As ClauseType, ByVal firstOperand As CriteriaOperator, ByVal operands As ICollection(Of CriteriaOperator)) As IClauseNode
            Dim [property] As OperandProperty = TryCast(firstOperand, OperandProperty)
            If Not ReferenceEquals([property], Nothing) Then
                Return Factory.Create(type, [property], operands)
            End If
            Dim aggregate As AggregateOperand = If(FactoryEx IsNot Nothing, TryCast(firstOperand, AggregateOperand), Nothing)
            If Not ReferenceEquals(aggregate, Nothing) Then
                Dim condition As INode = CriteriaToTreeProcessor.GetTree(Factory, aggregate.Condition, Skipped)
                If Not (TypeOf aggregate.AggregatedExpression Is OperandProperty) Then
                    Skip(aggregate.AggregatedExpression)
                End If
                Return FactoryEx.Create(aggregate.CollectionProperty, aggregate.AggregateType, TryCast(aggregate.AggregatedExpression, OperandProperty), type, operands, condition)
            End If
            Return Skip(origionalOperator)
        End Function



        Private Function IsGoodForAdditionalOperands(ByVal opa As CriteriaOperator) As Boolean
            If TypeOf opa Is OperandValue Then Return True
            If TypeOf opa Is OperandProperty Then Return True
            If TypeOf opa Is BinaryOperator Then
                Dim bo As BinaryOperator = TryCast(opa, BinaryOperator)
                If bo Is Nothing Then Return False
                Return IsGoodForAdditionalOperands(bo.RightOperand) AndAlso IsGoodForAdditionalOperands(bo.LeftOperand)

            ElseIf TypeOf opa Is FunctionOperator Then
                Dim fo As FunctionOperator = TryCast(opa, FunctionOperator)
                If fo Is Nothing Then Return False
                Select Case fo.OperatorType
                    Case FunctionOperatorType.LocalDateTimeThisYear, FunctionOperatorType.LocalDateTimeThisMonth, FunctionOperatorType.LocalDateTimeLastWeek, FunctionOperatorType.LocalDateTimeThisWeek, FunctionOperatorType.LocalDateTimeYesterday, FunctionOperatorType.LocalDateTimeToday, FunctionOperatorType.LocalDateTimeNow, FunctionOperatorType.LocalDateTimeTomorrow, FunctionOperatorType.LocalDateTimeDayAfterTomorrow, FunctionOperatorType.LocalDateTimeNextWeek, FunctionOperatorType.LocalDateTimeTwoWeeksAway, FunctionOperatorType.LocalDateTimeNextMonth, FunctionOperatorType.LocalDateTimeNextYear
                        Return True
                    Case FunctionOperatorType.Custom
                        Dim filterInfo As XpoFunctionFilterInfo = XpoFunctionFilterManager.GetInfos(DirectCast(fo.Operands(0), OperandValue).Value.ToString)
                        Return filterInfo IsNot Nothing
                End Select
            End If


            Return False
        End Function

        Private Function CreateNodeForUnaryClause(ByVal theOperator As FunctionOperator, ByVal clauseType As ClauseType) As INode
            If theOperator.Operands.Count <> 1 Then
                Return Me.Skip(theOperator)
            End If
            Return Me.CreateClauseNode(theOperator, clauseType, theOperator.Operands(0), New CriteriaOperator() {})
        End Function

        Private Function DoStartsEndsContains(ByVal opa As FunctionOperator) As INode
            If opa.Operands.Count <> 2 Then
                Return Me.Skip(opa)
            End If
            Dim firstOperand As CriteriaOperator = opa.Operands(0)
            Dim operator2 As CriteriaOperator = opa.Operands(1)
            If Not Me.IsGoodForAdditionalOperands(operator2) Then
                Return Me.Skip(opa)
            End If
            Select Case opa.OperatorType
                Case FunctionOperatorType.StartsWith
                    Return Me.CreateClauseNode(opa, ClauseType.BeginsWith, firstOperand, New CriteriaOperator() {operator2})

                Case FunctionOperatorType.EndsWith
                    Return Me.CreateClauseNode(opa, ClauseType.EndsWith, firstOperand, New CriteriaOperator() {operator2})

                Case FunctionOperatorType.Contains
                    Return Me.CreateClauseNode(opa, ClauseType.Contains, firstOperand, New CriteriaOperator() {operator2})
            End Select
            Throw New System.InvalidOperationException("Unexpected " & CriteriaOperator.ToString(opa))
        End Function

        Private Function VisitCustomFunction(ByVal theOperator As FunctionOperator) As INode
            Dim functionNameOperand As ConstantValue = CType(theOperator.Operands(0), ConstantValue)
            Dim functionName As String = DirectCast(functionNameOperand.Value, String)
            Select Case functionName
                Case "MatchesAnyOf"
                    Return Me.CreateClauseNode(theOperator, CType(ClauseTypeEnumHelper.MatchesAnyOf, ClauseType), theOperator.Operands(1), theOperator.Operands.Skip(2).ToList())
                Case Else
                    Dim filterInfo As XpoFunctionFilterInfo = XpoFunctionFilterManager.GetInfos(functionName)
                    If filterInfo IsNot Nothing Then

                        Return Me.CreateClauseNode(theOperator, CType(filterInfo.Id, ClauseType), theOperator.Operands(1), theOperator.Operands.Skip(2).ToList())
                    Else
                        Return Me.Skip(theOperator)
                        'Return Me.CreateClauseNode(New OperandValue, CType(filterInfo.Id, ClauseType), theOperator.Operands(1), theOperator.Operands.Skip(2).ToList())
                        'Return Nothing
                        Throw New System.NotSupportedException(DirectCast(functionNameOperand.Value, String))
                    End If
            End Select
        End Function

        Public Shared Shadows Function GetTree(ByVal nodesFactory As INodesFactory, ByVal op As CriteriaOperator, ByVal skippedCriteria As IList(Of CriteriaOperator)) As INode
            Return (New CriteriaToTreeProcessorEx(nodesFactory, skippedCriteria)).Process(op)
        End Function

        Private Function Process(ByVal op As CriteriaOperator) As INode
            If Object.ReferenceEquals(op, Nothing) Then
                Return Nothing
            End If
            Return op.Accept(Of INode)(Me)
        End Function
    End Class

End Namespace
