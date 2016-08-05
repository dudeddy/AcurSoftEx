Imports System.Reflection
Imports AcurSoft.Data.Filtering
Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.Data.Utils
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.XtraEditors.Filtering
    Public Class ClauseNodeEx
        Inherits ClauseNode
        Public Const FunctionsButtonTag As String = "     " '5 spaces

        Public ReadOnly Property FocusInfoEx As FilterControlFocusInfo
            Get
                Return Me.FocusInfo
            End Get
        End Property




        Public Function GetElementAt(index As Integer) As NodeEditableElementEx
            Return DirectCast(Me.Elements(index), NodeEditableElementEx)
        End Function
        Public Function GetFocusedElement() As NodeEditableElementEx
            Return Me.GetElementAt(Me.FocusInfoEx.ElementIndex)
        End Function

        Public ReadOnly Property ModelEx As WinFilterTreeNodeModelEx


        Public Sub New(ByVal model As WinFilterTreeNodeModelEx)
            MyBase.New(model)
            Me.ModelEx = model
        End Sub

        Public ReadOnly Property IsCustomFunction As Boolean
            Get
                Return Not Operation.IsDefined
            End Get
        End Property

        Public ReadOnly Property CustomFunctionInfo As XpoFunctionFilterInfo
            Get
                If Me.Operation.IsDefined Then Return Nothing
                Return XpoFunctionFilterManager.GetInfos(Me.Operation.IntegerValue)
            End Get
        End Property

        Public Function GetCustomFunctionParameterInfo(elementIndex As Integer) As XpoFunctionParamererFilterInfo
            If Not Me.IsCustomFunction Then Return Nothing
            Return Me.CustomFunctionInfo.GetFilterParameterInfo(elementIndex)
        End Function

        Public Function GetFocusedCustomFunctionParameterInfo() As XpoFunctionParamererFilterInfo
            If Not Me.IsCustomFunction Then Return Nothing
            Return Me.GetCustomFunctionParameterInfo(Me.FocusInfo.ElementIndex)
        End Function

        Private Shadows ReadOnly Property CanAddCollectionItem() As Boolean
            Get
                Return IsCollectionValues AndAlso Not IsShowCollectionValueAsOnEditor
            End Get
        End Property

        Private Shadows ReadOnly Property IsCollectionValues() As Boolean
            Get
                Dim x = MyBase.IsCollectionValues
                'Return Operation = ClauseType.AnyOf OrElse Operation = ClauseType.NoneOf OrElse Operation = CType(ClauseTypeEnumHelper.MatchesAnyOf, ClauseType)
                If Operation = ClauseType.AnyOf OrElse Operation = ClauseType.NoneOf Then
                    Return True
                ElseIf Operation = CType(ClauseTypeEnumHelper.MatchesAnyOf, ClauseType) Then
                    Return True
                Else
                    Return False
                End If

            End Get
        End Property

        Private ReadOnly Property AsShowCollectionValueAsOnEditor() As Boolean
            Get
                If [Property] Is Nothing Then Return False
                Return IsCollectionValues AndAlso Model.DoesAllowItemCollectionEditor([Property]) AndAlso AdditionalOperands.Count > Model.MaxOperandsCount
            End Get
        End Property

        Private Shadows ReadOnly Property IsTwoFieldsClause() As Boolean
            Get
                Return Operation = ClauseType.Between OrElse Operation = ClauseType.NotBetween
            End Get
        End Property

        Public Sub ChangeElementEx(ByVal element As NodeEditableElement, ByVal value As Object)
            Me.ChangeElement(element, value)
        End Sub

        Public Sub NodeChangedEx(Optional filterChangedAction As FilterChangedAction = FilterChangedAction.ValueChanged)
            Me.NodeChanged(filterChangedAction)
        End Sub



        Protected Overrides Sub ChangeElement(ByVal element As NodeEditableElement, ByVal value As Object)
            'If DirectCast(element, NodeEditableElementEx).IsParameter Then
            '    Model.BeginUpdate()
            '    Operation = DirectCast(value, ClauseType)
            '    Dim clauseType As Integer = DirectCast(value, Integer)
            '    ValidateAdditionalOperands(clauseType)
            '    Model.EndUpdate(FilterChangedAction.OperationChanged, Me)
            '    Dim fi As FilterControlFocusInfo = FocusInfo.OnRight()
            '    If fi.Node Is FocusInfo.Node Then
            '        FocusInfo = fi
            '    End If
            '    Return
            'End If
            Dim eleEx As NodeEditableElementEx = DirectCast(element, NodeEditableElementEx)
            Select Case element.ElementType
                Case ElementType.Operation
                    If Not System.Enum.IsDefined(GetType(ClauseType), value) Then
                        Model.BeginUpdate()
                        Operation = DirectCast(value, ClauseType)
                        Dim clauseType As Integer = DirectCast(value, Integer)
                        ValidateAdditionalOperands(clauseType)
                        Model.EndUpdate(FilterChangedAction.OperationChanged, Me)
                        Dim fi As FilterControlFocusInfo = FocusInfo.OnRight()
                        If fi.Node Is FocusInfo.Node Then
                            FocusInfo = fi
                        End If
                    Else
                        MyBase.ChangeElement(element, value)
                    End If
                Case ElementType.CollectionAction
                    Model.BeginUpdate()
                    AdditionalOperands.Add(New OperandValue())
                    Model.EndUpdate()
                    Dim focusIndex As Integer = GetElement(ElementType.Operation).Index + 1
                    If Not IsShowCollectionValueAsOnEditor Then
                        focusIndex += AdditionalOperands.Count - 1
                    End If
                    NodeChanged(FilterChangedAction.ValueChanged)
                    FocusInfo = New FilterControlFocusInfo(Me, focusIndex)
                Case ElementType.Value
                    If eleEx.IsParameter OrElse eleEx.IsValueElement Then
                        Model.BeginUpdate()
                        eleEx.ChangeParameterValue(New OperandValue(value))
                        Model.EndUpdate()
                        NodeChanged(FilterChangedAction.ValueChanged)
                    Else
                        MyBase.ChangeElement(element, value)
                    End If
                Case Else
                    MyBase.ChangeElement(element, value)
            End Select
        End Sub

        Private Overloads Sub ValidateAdditionalOperands(ByVal clauseType As Integer)
            ' Usage: test the clauseType argument, and call the FilterHelper.ForceAdditionalParamsCount method or do nothing depending on
            ' how many parameters the specific filter operation requires.
            ' FilterControlHelpers.ForceAdditionalParamsCount(AdditionalOperands, n); - declares n operands, where n is an integer value
            ' FilterControlHelpers.ForceAdditionalParamsCount(AdditionalOperands, 0); - declares no operands
            ' If the operation supports arbitrary amount of operands, do nothing
            'Dim info As CriteriaFunctionBaseInfo = XpoFunctionsHelper.GetStoredCriteriaFunctionInfo(clauseType)
            Dim info As XpoFunctionFilterInfo = XpoFunctionFilterManager.GetInfos(clauseType)
            If info IsNot Nothing Then
                FilterControlHelpers.ForceAdditionalParamsCount(AdditionalOperands, info.XpoFunction.MaxOperandCount)
            ElseIf clauseType = ClauseTypeEnumHelper.MatchesAnyOf Then
                Return
            End If
        End Sub

        Public ReadOnly Property FirstOperandElement As NodeEditableElementEx
        Public ReadOnly Property OperationElement As NodeEditableElementEx

        Public Overrides Sub RebuildElements()
            Elements.Clear()
            _FirstOperandElement = NodeParameterValueElementEx.CreatePropertyElement(Me, Me.FirstOperand)

            '_FirstOperandElement = AddEditableElement(ElementType.Property, GetDisplayText(FirstOperand))
            _FirstOperandElement.TextAfter = " "
            BuildOperationAndValueElements()
        End Sub

        Private Shadows Sub BuildOperationAndValueElements()
            '_OperationElement = AddEditableElement(ElementType.Operation, ModelEx.GetMenuStringByType(Operation))
            _OperationElement = NodeOperationElementEx.CreateOperationElement(Me, Me.Operation)
            _OperationElement.TextAfter = " "
            BuildValueElements()
            If CanAddCollectionItem Then
                NodeActionElementEx.CreateCollectionActionElement(Me)
                'AddEditableElement(ElementType.CollectionAction, "@+")
            End If
            NodeActionElementEx.CreateNodeRemoveElement(Me)

            'AddEditableElement(ElementType.NodeRemove, "@-")
        End Sub

        Public Overrides Sub AddElement()
            Dim element As NodeEditableElement = GetElement(FocusInfo.ElementIndex)
            If IsCollectionValues AndAlso element IsNot Nothing AndAlso (element.IsValueElement OrElse element.ElementType = ElementType.ItemCollection OrElse element.ElementType = ElementType.CollectionAction OrElse element.ElementType = ElementType.Operation) Then
                ChangeElement(ElementType.CollectionAction)
            Else
                CType(ParentNode, Node).AddElement()
            End If
        End Sub

        Public Sub SetFocus(index As Integer)
            FocusInfo = New FilterControlFocusInfo(Me, index)
        End Sub

        Public Function GetFocusInfo() As FilterControlFocusInfo
            Return FocusInfo
        End Function
        Public Overrides Sub DeleteElement()
            Dim element As NodeEditableElement = GetElement(FocusInfo.ElementIndex)
            If IsCollectionValues AndAlso element IsNot Nothing AndAlso (element.IsValueElement OrElse element.ElementType = ElementType.ItemCollection) Then
                If element.ElementType <> ElementType.ItemCollection AndAlso element.ValueIndex >= AdditionalOperands.Count - 1 Then
                    FocusInfo = New FilterControlFocusInfo(Me, element.Index - 1)
                End If
                AdditionalOperands.RemoveAt(element.ValueIndex)
            Else
                Dim parent As GroupNode = TryCast(ParentNode, GroupNode)
                If parent Is Nothing Then Return
                Dim indexOfFocusedNode As Integer = parent.GetChildren().IndexOf(FocusInfo.Node)
                parent.GetChildren().RemoveAt(indexOfFocusedNode)
                If indexOfFocusedNode >= parent.GetChildren().Count Then
                    indexOfFocusedNode = parent.GetChildren().Count - 1
                End If
                If indexOfFocusedNode >= 0 Then
                    FocusInfo = New FilterControlFocusInfo(DirectCast(parent.GetChildren()(indexOfFocusedNode), Node), 0)
                Else
                    FocusInfo = New FilterControlFocusInfo(parent, 0)
                End If
            End If
        End Sub

        'Public Shadows Function AddEditableElement(ByVal elementType As ElementType, ByVal text As String, criteria As CriteriaOperator) As NodeEditableElementEx
        '    Dim element As New NodeEditableElementEx(Me, elementType, text, -1, criteria)
        '    Elements.Add(element)
        '    Return element
        'End Function


        'Public Shadows Function AddEditableElement(ByVal elementType As ElementType, ByVal text As String, elementNodeIndex As Integer, criteria As CriteriaOperator) As NodeEditableElementEx
        '    Dim element As New NodeEditableElementEx(Me, elementType, text, elementNodeIndex, criteria)
        '    Elements.Add(element)
        '    Return element
        'End Function


        'Public Shadows Function AddEditableElement(ByVal elementType As ElementType, ByVal text As String, elementNodeIndex As Integer) As NodeEditableElementEx
        '    Dim element As New NodeEditableElementEx(Me, elementType, text, elementNodeIndex)
        '    Elements.Add(element)
        '    Return element
        'End Function


        'Public Shadows Function AddEditableElement(ByVal elementType As ElementType, ByVal text As String) As NodeEditableElementEx
        '    Dim element As New NodeEditableElementEx(Me, elementType, text, -1)
        '    Elements.Add(element)
        '    Return element
        'End Function


        'Public Sub ReBuildValueElements()
        '    BuildValueElements()
        'End Sub

        Private Shadows Sub BuildValueElements()
            If AdditionalOperands Is Nothing Then Return
            If IsShowCollectionValueAsOnEditor Then
                'Dim str As String = CStr(GetType(ClauseNode).InvokeMember("GetCollectionValuesString", BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.InvokeMethod, Nothing, Me, New Object() {}))
                'AddEditableElement(ElementType.ItemCollection, str)
                NodeItemCollectionElementEx.CreateItemCollectionElement(Me)
                Return
            End If

            Dim cnt As Integer = Me.AdditionalOperands.Count - 1

            Dim parentheseOpen As New NodeParentheseElementEx(Me, NodeParentheseKindEnum.Open) With {.Opposite = New NodeParentheseElementEx(Me, NodeParentheseKindEnum.Close)}
            Dim parentheseClose As NodeParentheseElementEx = parentheseOpen.Opposite
            parentheseClose.Opposite = parentheseOpen


            If Me.IsCollectionValues OrElse Me.AdditionalOperands.Count > 1 Then
                If Me.OperationElement IsNot Nothing Then
                    Me.OperationElement.TextAfter = ""
                End If
                Me.Elements.Add(parentheseOpen)
            End If

            For i As Integer = 0 To cnt
                Dim op As CriteriaOperator = AdditionalOperands(i)
                Dim opKind As CriteriaKind = op.GetCritriaKind
                Select Case opKind
                    Case CriteriaKind.OperandValue
                        With New NodeParameterValueElementEx(Me, DirectCast(op, OperandValue), -1)
                            .ElementNodeIndex = i
                            .ValueIndex = i
                            Me.Elements.AddRange(.Elements)
                            'If Me.IsCustomFunction Then
                            '    Me.Elements.AddRange(.Elements.Take(1))
                            'Else
                            '    Me.Elements.AddRange(.Elements)
                            'End If
                        End With
                    Case CriteriaKind.OperandProperty
                        With New NodeParameterValueElementEx(Me, DirectCast(op, OperandProperty), -1)
                            .ElementNodeIndex = i
                            .ValueIndex = i
                            Me.Elements.AddRange(.Elements)
                            'If Me.IsCustomFunction Then
                            '    Me.Elements.AddRange(.Elements.Take(1))
                            'Else
                            '    Me.Elements.AddRange(.Elements)
                            'End If

                        End With
                    Case CriteriaKind.FunctionOperator
                        With New NodeFunctionElementEx(Me, i, DirectCast(op, FunctionOperator))
                            .ElementNodeIndex = i
                            Me.Elements.AddRange(.BuildElements)
                        End With
                    Case CriteriaKind.BinaryOperator
                        Dim bo As BinaryOperator = DirectCast(op, BinaryOperator)
                        If bo.IsMathOperation Then
                            With New NodeMathElementEx(Me, i, bo)
                                .ElementNodeIndex = i
                                Me.Elements.AddRange(.BuildElements)
                            End With
                        ElseIf bo.IsEqualityOperation
                            With New NodeEqualityElementEx(Me, i, bo)
                                .ElementNodeIndex = i
                                Me.Elements.AddRange(.BuildElements)
                            End With
                        End If
                End Select

                If i <> cnt Then
                    Me.Elements.Add(New NodeCommaElementEx(Me))
                End If
            Next
            If Me.IsCollectionValues OrElse Me.AdditionalOperands.Count > 1 Then
                Me.Elements.Add(parentheseClose)
            End If

            'Dim xpoFunction As XpoFunctionBase = Nothing
            'Dim customFunction As ICustomFunctionOperator = Nothing
            'Dim str As String = StringAdaptation(GetDisplayText(FirstOperand, op))
            'Dim customFunctionString As String = str
            'If str Is Nothing OrElse str.Length = 0 Then
            '    str = "''"
            'End If
            'Dim elementType As ElementType = ElementType.AdditionalOperandProperty
            'Dim isEmpty As Boolean = Not ReferenceEquals(op, Nothing) AndAlso op.ToString() = "?"
            'If TypeOf op Is OperandParameter Then
            '    elementType = ElementType.AdditionalOperandParameter
            '    If isEmpty Then
            '        str = Model.GetLocalizedStringForFilterEmptyParameter()
            '    End If
            'ElseIf TypeOf op Is OperandValue Then
            '    elementType = ElementType.Value
            '    If isEmpty Then
            '        str = Model.GetLocalizedStringForFilterEmptyEnter()
            '    End If
            'End If
            'Dim element As NodeEditableElementEx = Nothing
            'Dim cr As CriteriaOperator = Me.AdditionalOperands(i)
            'If TypeOf cr Is FunctionOperator Then
            '    'element = New NodeEditableElementEx(Me, elementType, str, i)
            '    'element.Init(False)

            '    'Dim fe As New NodeFunctionElementEx(Me, i, DirectCast(cr, FunctionOperator))
            '    'Dim elms As List(Of NodeEditableElementEx) = fe.BuildElements
            '    'Me.Elements.AddRange(fe.BuildElements)
            '    'For Each elm In elms
            '    'Next


            '    'If fe.CanBuildElements Then

            '    'End If

            'ElseIf TypeOf cr Is BinaryOperator Then
            '    element = New NodeEditableElementEx(Me, elementType, str, i)
            '    element.Init(False)
            'Else
            '    'element = Me.AddEditableElement(elementType, str, i)
            '    'element.IsEmpty = isEmpty
            '    'element.ValueIndex = i
            '    'element.Init()

            'End If

            'If element IsNot Nothing Then


            '    Dim operandElement As NodeEditableElementEx = Nothing
            '    Dim fncButtonElement As NodeEditableElementEx = Nothing
            '    If ShowOperandTypeIcon Then
            '        operandElement = AddEditableElement(ElementType.FieldAction, "@#")
            '        operandElement.ValueIndex = i
            '        If Not element.IsFunction Then

            '        End If
            '    End If
            '    If IsTwoFieldsClause AndAlso i = 1 Then
            '        element.TextBefore = " " & Model.GetLocalizedStringForFilterClauseBetweenAnd() & " "
            '    End If
            '    If AdditionalOperands.Count = 1 Then
            '        If Not element.IsFunction Then
            '        End If
            '        fncButtonElement = AddEditableElement(DirectCast(FieldActionEx.ShowFunctions, ElementType), ClauseNodeEx.FunctionsButtonTag)
            '    End If
            '    If IsCollectionValues OrElse AdditionalOperands.Count > 1 Then
            '        If i = 0 Then
            '            element.TextBefore = "( "
            '            If Not element.IsFunction Then
            '            End If
            '            fncButtonElement = AddEditableElement(DirectCast(FieldActionEx.ShowFunctions, ElementType), ClauseNodeEx.FunctionsButtonTag)

            '            If AdditionalOperands.Count > 1 Then
            '                fncButtonElement.TextAfter = ", "
            '            End If
            '        End If
            '        If i > 0 Then
            '            'element.TextBefore = ", "
            '            If i = AdditionalOperands.Count - 1 Then
            '                fncButtonElement = AddEditableElement(DirectCast(FieldActionEx.ShowFunctions, ElementType), ClauseNodeEx.FunctionsButtonTag)
            '                fncButtonElement.TextAfter = " )"
            '            Else
            '                fncButtonElement = AddEditableElement(DirectCast(FieldActionEx.ShowFunctions, ElementType), ClauseNodeEx.FunctionsButtonTag)
            '                If i < AdditionalOperands.Count - 1 Then
            '                    fncButtonElement.TextAfter = ", "
            '                End If
            '            End If
            '            If Not element.IsFunction Then
            '            End If
            '        End If
            '        'If i = AdditionalOperands.Count - 1 Then
            '        '    If operandElement IsNot Nothing Then
            '        '        'operandElement.TextAfter = ")"
            '        '        AddEditableElement(DirectCast(FieldActionEx.ShowFunctions, ElementType), "     ").TextAfter = ")"
            '        '    Else
            '        '        'element.TextAfter = ")"
            '        '        AddEditableElement(DirectCast(FieldActionEx.ShowFunctions, ElementType), "     ").TextAfter = ")"
            '        '    End If
            '        'End If
            '    End If
            'End If
            'Next i
        End Sub

        Public Function GetFirstOperandType() As Type
            Dim firstOperandName As String = Me.FirstOperand.PropertyName.Trim.ToUpper
            Return Me.Model.FilterProperties.OfType(Of IBoundProperty).FirstOrDefault(Function(q) q.Name.ToUpper() = firstOperandName)?.Type
        End Function

    End Class

End Namespace
