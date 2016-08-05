Imports System.Reflection
Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.Data.Utils
Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors.Filtering
Imports DevExpress.XtraEditors.Repository

Namespace AcurSoft.Data.Filtering
    Public Enum ButtonKindEnum
        None = 0
        Editor = 1
        Column = 2
        [Function] = 3
        Open = 4
        Close = 5
    End Enum

    Public Enum NodeElementKindEnum
        None = 0
        Editor = 1
        Column = 2
        [Function] = 3
        Math = 4
        ParentheseOpen = 20
        ParentheseClose = 21
    End Enum

    Public MustInherit Class NodeEditableElementEx
        Inherits NodeEditableElement

        Public ReadOnly Property NodeEx As ClauseNodeEx
        Public Overridable ReadOnly Property NodeKind As NodeElementKindEnum = NodeElementKindEnum.None
        Public Overridable Property Criteria As CriteriaOperator
        Public Overridable ReadOnly Property NeedReprocessing As Boolean
        Public Property ElementNodeIndex As Integer
        Public Property RootElementIndex As Integer

        Public Property ParentElement As NodeEditableElementEx

        Public Property ParameterOf As NodeEditableElementEx

        Public Property ParameterIndex As Integer
        Public Property CriteriaIndex As Integer
        Public ReadOnly Property IsParameter As Boolean
            Get
                Return Me.ParameterIndex <> -1
            End Get
        End Property

        Public ReadOnly Property IsFunction As Boolean
            Get
                Return TypeOf Me Is NodeFunctionElementEx
            End Get
        End Property

        Private _CleanElement As ElementType
        Public Sub SetElementType(et As ElementType)
            Dim fi As FieldInfo = GetType(NodeEditableElement).GetField("elementType", BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
            If fi IsNot Nothing Then
                fi.SetValue(Me, et)
            End If
        End Sub



        Public Sub ResetElementType()
            Dim fi As FieldInfo = GetType(NodeEditableElement).GetField("elementType", BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
            If fi IsNot Nothing Then
                fi.SetValue(Me, _CleanElement)
            End If
        End Sub
        Public Property ButtonKind As ButtonKindEnum = ButtonKindEnum.None

        Public Function GetParameterElements() As List(Of NodeEditableElementEx)
            Return Me.NodeEx.Elements.OfType(Of NodeEditableElementEx).Where(Function(q) q.ParameterOf Is Me AndAlso q.ButtonKind = ButtonKindEnum.None).ToList()
        End Function

        Public Overridable Function GetParameterEditor() As RepositoryItem
            If Me.IsParameter AndAlso Me.ParameterOf IsNot Nothing AndAlso Me.ParameterOf.IsFunction AndAlso DirectCast(Me.ParameterOf, NodeFunctionElementEx).XpoFunctionFilterInfo IsNot Nothing Then
                Return DirectCast(Me.ParameterOf, NodeFunctionElementEx).XpoFunctionFilterInfo.Parameters(Me.ParameterIndex).EditorCreator.Invoke()
            End If
            Return Nothing
        End Function



        Public Sub New(ByVal node As ClauseNodeEx, ByVal elementType As ElementType, ByVal text As String, elementNodeIndex As Integer)
            Me.New(node, elementType, text)
            Me.ElementNodeIndex = elementNodeIndex
            If elementNodeIndex >= 0 AndAlso elementNodeIndex <= node.AdditionalOperands.Count - 1 Then
                Me.Criteria = node.AdditionalOperands(elementNodeIndex)
            End If
        End Sub

        'Public Sub New(ByVal node As ClauseNodeEx, ByVal elementType As ElementType, ByVal text As String, criteria As CriteriaOperator)
        '    Me.New(node, elementType, text, -1)
        '    Me.Criteria = criteria
        'End Sub

        'Public Sub New(ByVal node As ClauseNodeEx, ByVal elementType As ElementType, ByVal text As String, elementNodeIndex As Integer, criteria As CriteriaOperator)
        '    Me.New(node, elementType, text, elementNodeIndex)
        '    Me.Criteria = criteria
        'End Sub

        'Public Sub SetText(txt As String)
        '    Dim fi As FieldInfo = GetType(NodeEditableElement).GetField("text", BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
        '    If fi IsNot Nothing Then
        '        fi.SetValue(Me, txt)
        '    End If
        'End Sub

        'Public Sub ResetCriteria()

        'End Sub
        Private _TextFieldInfo As FieldInfo

        Private _Text As String
        Public Shadows Property Text As String
            Get
                Return _Text
            End Get
            Set(value As String)
                _TextFieldInfo.SetValue(Me, value)
                _Text = value
            End Set
        End Property


        Public Sub New(ByVal node As ClauseNodeEx, ByVal elementType As ElementType, ByVal text As String)
            MyBase.New(node, elementType, text)
            _TextFieldInfo = GetType(NodeEditableElement).GetField("text", BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
            Me.Text = text

            _ParameterIndex = -1
            _CriteriaIndex = -1
            _RootElementIndex = -1
            _CleanElement = elementType
            Me.ElementNodeIndex = -1
            'Me.Text = text
            _NodeEx = node
        End Sub

        Public ReadOnly Property ParentElementIndex As Integer
            Get
                If Me.ParentElement Is Nothing Then Return -1
                Return Me.ParentElement.Index
            End Get
        End Property

        Public Function GetChildren() As List(Of NodeEditableElementEx)
            Return Me.NodeEx.Elements.OfType(Of NodeEditableElementEx).Where(Function(q) q.ParentElementIndex = Me.Index).ToList
        End Function

        Public ReadOnly Property RootElement As NodeEditableElementEx
            Get
                If Me.ParameterOf Is Nothing Then Return Me
                Return Me.ParameterOf.RootElement
            End Get
        End Property

        Public Sub ChangeParameterValue(value As CriteriaOperator)
            If Me.IsParameter AndAlso Me.ParameterOf IsNot Nothing Then
                Dim paramIndex As Integer = Me.ParameterIndex
                Dim paramElement As NodeEditableElementEx = Me.ParameterOf.GetParameterElements(Me.ParameterIndex)

                Dim root As NodeEditableElementEx = paramElement.RootElement
                If TypeOf root Is INodeConvertibleToCriteria Then
                    paramElement.Criteria = value
                    Me.NodeEx.AdditionalOperands(Me.ParameterOf.ElementNodeIndex) = DirectCast(root, INodeConvertibleToCriteria).ToCriteria
                    Return
                End If
                paramElement.IsEmpty = value IsNot Nothing AndAlso value.ToString() = "?"
                If paramElement.IsEmpty Then
                    'paramElement.SetText(Me.NodeEx.Model.GetLocalizedStringForFilterEmptyParameter())
                    paramElement.Text = Me.NodeEx.Model.GetLocalizedStringForFilterEmptyEnter()
                ElseIf value.GetCritriaKind = CriteriaKind.OperandValue
                    Dim v As Object = DirectCast(value, OperandValue)
                    If v.GetType.IsCastableTo(Of Integer) Then
                        If CType(v, Integer) = CType(v, Decimal) Then
                            Me.Text = CType(v, Integer).ToString
                        End If
                    End If
                End If
            ElseIf Me.IsValueElement
                Me.SetElementValue(value)
                'Me.Criteria = value
                'Me.NodeEx.AdditionalOperands(Me.ElementNodeIndex) = value
                'Me.IsEmpty = value IsNot Nothing AndAlso value.ToString() = "?"
                'If Me.IsEmpty Then
                '    'Me.SetText(Me.NodeEx.Model.GetLocalizedStringForFilterEmptyParameter())
                '    Me.Text = Me.NodeEx.Model.GetLocalizedStringForFilterEmptyParameter()
                'ElseIf value.GetCritriaKind = CriteriaKind.OperandValue
                '    Dim v As Object = DirectCast(value, OperandValue)
                '    If v.GetType.IsCastableTo(Of Integer) Then
                '        If CType(v, Integer) = CType(v, Decimal) Then
                '            Me.Text = CType(v, Integer).ToString
                '        End If
                '    End If
                'End If

            End If
        End Sub

        Public Sub SetElementValue(value As CriteriaOperator)
            Me.Criteria = value
            Me.NodeEx.AdditionalOperands(Me.ElementNodeIndex) = value
            Me.IsEmpty = value IsNot Nothing AndAlso value.ToString() = "?"
            If Me.IsEmpty Then
                'Me.SetText(Me.NodeEx.Model.GetLocalizedStringForFilterEmptyParameter())
                Me.Text = Me.NodeEx.Model.GetLocalizedStringForFilterEmptyEnter
            ElseIf value.GetCritriaKind = CriteriaKind.OperandValue
                Dim v As Object = DirectCast(value, OperandValue)
                If v.GetType.IsCastableTo(Of Integer) Then
                    If CType(v, Integer) = CType(v, Decimal) Then
                        Me.Text = CType(v, Integer).ToString
                    End If
                End If
            End If

        End Sub

    End Class
End Namespace
