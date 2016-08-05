Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.XtraEditors.Filtering
Namespace AcurSoftTests

    Public NotInheritable Class NodesFactoryAdv
        Implements INodesFactory

        Private Function Create(ByVal type As ClauseType, ByVal firstOperand As OperandProperty, ByVal operands As ICollection(Of CriteriaOperator)) As IClauseNode Implements INodesFactory.Create
            Dim result As IClauseNode = New ClauseNodeAdv(firstOperand, type)
            For Each operand As CriteriaOperator In operands
                result.AdditionalOperands.Add(operand)
            Next operand
            Return result
        End Function

        Private Function Create(ByVal type As GroupType, ByVal subNodes As ICollection(Of INode)) As IGroupNode Implements INodesFactory.Create
            Dim result As IGroupNode = New GroupNodeAdv(type)
            For Each subNode As INode In subNodes
                result.SubNodes.Add(subNode)
                subNode.SetParentNode(result)
            Next subNode
            Return result
        End Function
    End Class
    Public NotInheritable Class ClauseNodeAdv
        Implements IClauseNode

        Private ReadOnly fAdditionalOperands As IList(Of CriteriaOperator)
        Private ReadOnly fFirstOperand As OperandProperty
        Private ReadOnly fOperation As ClauseType
        Private fParentNode As IGroupNode

        Public Sub New(ByVal firstOperand As OperandProperty, ByVal operation As ClauseType)
            Me.fAdditionalOperands = New List(Of CriteriaOperator)()
            Me.fFirstOperand = firstOperand
            Me.fOperation = operation
            Me.fParentNode = Nothing
        End Sub

        Private ReadOnly Property AdditionalOperands() As IList(Of CriteriaOperator) Implements IClauseNode.AdditionalOperands
            Get
                Return Me.fAdditionalOperands
            End Get
        End Property

        Private ReadOnly Property FirstOperand() As OperandProperty Implements IClauseNode.FirstOperand
            Get
                Return Me.fFirstOperand
            End Get
        End Property

        Private ReadOnly Property Operation() As ClauseType Implements IClauseNode.Operation
            Get
                Return Me.fOperation
            End Get
        End Property

        Private Function INode_Accept(ByVal visitor As INodeVisitor) As Object Implements INode.Accept
            Return visitor.Visit(Me)
        End Function

        Private ReadOnly Property ParentNode() As IGroupNode Implements INode.ParentNode
            Get
                Return Me.fParentNode
            End Get
        End Property

        Private Sub SetParentNode(ByVal parentNode As IGroupNode) Implements INode.SetParentNode
            Me.fParentNode = parentNode
        End Sub
    End Class
    Public NotInheritable Class GroupNodeAdv
        Implements IGroupNode

        Private ReadOnly fNodeType As GroupType
        Private ReadOnly fSubNodes As IList(Of INode)
        Private fParentNode As IGroupNode

        Public Sub New(ByVal nodeType As GroupType)
            Me.fNodeType = nodeType
            Me.fSubNodes = New List(Of INode)()
            Me.fParentNode = Nothing
        End Sub

        Private ReadOnly Property NodeType() As GroupType Implements IGroupNode.NodeType
            Get
                Return Me.fNodeType
            End Get
        End Property

        Private ReadOnly Property SubNodes() As IList(Of INode) Implements IGroupNode.SubNodes
            Get
                Return Me.fSubNodes
            End Get
        End Property

        Private Function Accept(ByVal visitor As INodeVisitor) As Object Implements INode.Accept
            Return visitor.Visit(Me)
        End Function

        Private ReadOnly Property ParentNode() As IGroupNode Implements INode.ParentNode
            Get
                Return Me.fParentNode
            End Get
        End Property

        Private Sub SetParentNode(ByVal parentNode As IGroupNode) Implements INode.SetParentNode
            Me.fParentNode = parentNode
        End Sub
    End Class
End Namespace
