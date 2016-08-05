Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.Data.Filtering

    Public Enum NodeParameterValueKindEnum
        Value
        [Property]
        Parameter
    End Enum

    Public Class NodeParameterValueElementEx
        Inherits NodeEditableElementEx
        Implements INodeConvertibleToCriteria


        Public ReadOnly Property ParameterKind As NodeParameterValueKindEnum


        Private _Elements As List(Of NodeEditableElementEx)
        Public ReadOnly Property Elements As List(Of NodeEditableElementEx)
            Get
                If _Elements Is Nothing Then
                    _Elements = New List(Of NodeEditableElementEx)
                End If
                Return _Elements
            End Get
        End Property

        Public Shared Function CreatePropertyElement(ByVal node As ClauseNodeEx, op As OperandProperty, Optional parameterIndex As Integer = -1, Optional addToNode As Boolean = True) As NodeParameterValueElementEx
            Dim elm As New NodeParameterValueElementEx(node, op, parameterIndex)
            If addToNode Then
                node.Elements.Add(elm)
            End If
            Return elm
        End Function

        Public Sub New(ByVal node As ClauseNodeEx, op As OperandProperty, parameterIndex As Integer)
            MyBase.New(node, ElementType.Property, op.ToString)
            Me.ParameterKind = NodeParameterValueKindEnum.Property
            With Me.NodeEx.ModelEx.GetBoundProperty(op.PropertyName)
                If .HasValue Then
                    'Me.SetText(New OperandProperty(.Value.Name).ToString)
                    Me.Text = New OperandProperty(.Value.Name).ToString
                End If
            End With
            Me.Criteria = op
            Me.Elements.Add(Me)
            Me.Elements.Add(New NodeActionElementEx(Me))
        End Sub

        Public Sub New(ByVal parameterOf As NodeEditableElementEx, op As OperandProperty, parameterIndex As Integer)
            Me.New(parameterOf.NodeEx, op, parameterIndex)
            Me.ParameterOf = parameterOf
            Me.ParameterIndex = parameterIndex
        End Sub

        Public Sub New(ByVal node As ClauseNodeEx, op As OperandValue, parameterIndex As Integer)
            MyBase.New(node, ElementType.Value, op.ToString)
            Me.IsEmpty = Not ReferenceEquals(op, Nothing) AndAlso op.ToString() = "?"
            If Me.IsEmpty Then
                If TypeOf op Is OperandParameter Then
                    Me.SetElementType(ElementType.AdditionalOperandParameter)
                    'Me.SetText(Me.NodeEx.ModelEx.GetLocalizedStringForFilterEmptyParameter())
                    Me.Text = Me.NodeEx.ModelEx.GetLocalizedStringForFilterEmptyEnter()
                Else
                    'Me.SetText(Me.NodeEx.ModelEx.GetLocalizedStringForFilterEmptyEnter())
                    Me.Text = Me.NodeEx.ModelEx.GetLocalizedStringForFilterEmptyEnter()
                End If
                Me.ParameterKind = NodeParameterValueKindEnum.Parameter
            Else
                Me.ParameterKind = NodeParameterValueKindEnum.Value
                If op.GetCritriaKind = CriteriaKind.OperandValue Then
                    Dim v As Object = DirectCast(op, OperandValue).Value
                    If v.GetType.IsCastableTo(Of Integer) Then
                        If CType(v, Integer) = CType(v, Decimal) Then
                            Me.Text = CType(v, Integer).ToString
                        End If
                    End If

                End If
            End If
            Me.Criteria = op
            Me.Elements.Add(Me)
            Me.Elements.Add(New NodeActionElementEx(Me))
        End Sub


        Public Sub New(ByVal parameterOf As NodeEditableElementEx, op As OperandValue, parameterIndex As Integer)
            Me.New(parameterOf.NodeEx, op, parameterIndex)
            Me.ParameterOf = parameterOf
            Me.ParameterIndex = parameterIndex
        End Sub

        Public Function ToCriteria() As CriteriaOperator Implements INodeConvertibleToCriteria.ToCriteria
            Return Me.Criteria
        End Function
    End Class
End Namespace
