Imports System.ComponentModel
Imports System.Reflection
Imports AcurSoft.Data.Filtering
Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.Data.Utils
Imports AcurSoft.XtraEditors.Filtering
Imports AcurSoft.XtraEditors.Repository
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Utils.Frames
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Filtering
Imports DevExpress.XtraEditors.Repository

Namespace AcurSoft.XtraEditors

    Public Class FilterControlEx
        Inherits FilterControl

        Public Event OnCustomFunctionsPopupMenuShowing(ByVal e As PopupCustomFunctionsMenuShowingEventArgs)

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal container As IContainer)
            container.Add(Me)

            InitializeComponent()
        End Sub

        Protected Overrides Function CreatePainter() As Drawing.BaseControlPainter
            Return New FilterControlPainterEx(Me)
        End Function

        'Protected Overrides Sub OnClick(e As EventArgs)
        '    MyBase.OnClick(e)

        '    Dim p As Point = Me.FilterViewInfo.MousePosition
        '    Dim labelInfo As FilterControlLabelInfo = Me.Model.GetLabelInfoByCoordinates(p.X, p.Y)
        '    'Dim labelInfo2 As FilterControlLabelInfo = Me.RootNode.g(p.X, p.Y)
        '    'Me.Model..
        '    If labelInfo IsNot Nothing Then
        '        labelInfo.Refresh()
        '        'Dim isParenthesePressed As Boolean = False
        '        'Dim parentheseElement As NodeParentheseElementEx = Nothing
        '        'Dim parentheseElementOpposite As NodeParentheseElementEx = Nothing
        '        'Dim parentheseTextElement As FilterLabelInfoTextViewInfo = Nothing
        '        'Dim parentheseTextElementOpposite As FilterLabelInfoTextViewInfo = Nothing
        '        'Dim oppositeParentheseProcessed As Boolean = False
        '        'Dim xx = labelInfo.ViewInfo.Texts.OfType(Of LabelInfoText).FirstOrDefault(Function(q) q.Tag IsNot Nothing AndAlso TypeOf q.Tag Is NodeParentheseElementEx)

        '        'Dim xx = labelInfo.ViewInfo.Texts.Item(0).Tag
        '        'Dim ppp = 0
        '    End If

        'End Sub


        Private Sub OnClauseClick(ByVal sender As Object, ByVal e As EventArgs)
            Dim node As ClauseNode = CType(FocusInfo.Node, ClauseNode)
            node.Operation = DirectCast(DirectCast(sender, DXMenuItem).Tag, ClauseType)
            GetType(FilterControl).GetMethod("RefreshTreeAfterNodeChange", BindingFlags.Instance Or BindingFlags.NonPublic).Invoke(Me, Type.EmptyTypes)
            MyBase.Refresh(True, False)
            RaiseFilterChanged(New FilterChangedEventArgs(FilterChangedAction.OperationChanged, node))
            Dim fi As FilterControlFocusInfo = FocusInfo.OnRight()
            If fi.Node Is FocusInfo.Node Then
                FocusInfo = fi
            End If
        End Sub


        'Protected Overrides Function CreateModel() As WinFilterTreeNodeModel
        '    Return New WinFilterTreeNodeModelEx(Me)
        'End Function

        'Protected Overridable Sub RaiseCustomFunctionsPopupMenuShowing(ByVal e As PopupCustomFunctionsMenuShowingEventArgs)
        '    RaiseEvent OnCustomFunctionsPopupMenuShowing(e)
        '    Dim popup As New DXPopupStandardMenu(e.Menu)
        '    'Dim mm = Me.MenuManager
        '    e.Menu.Items.Add(New DXMenuItem("Matches any of") With {.Tag = ClauseTypeEnumHelper.MatchesAnyOf})

        '    If e.Cancel Then Return
        '    popup.Show(Me, e.Point)
        'End Sub

        Public Function SetActiveEditor(edit As BaseEdit) As BaseEdit
            Dim fi As FieldInfo = GetType(FilterControl).GetField("activeEditor", BindingFlags.Instance Or BindingFlags.NonPublic)
            fi.SetValue(Me, edit)
            Return edit
        End Function

        Public Sub SetSuspendEditorCreate(b As Boolean)
            Dim fi As FieldInfo = GetType(FilterControl).GetField("suspendEditorCreate", BindingFlags.Instance Or BindingFlags.NonPublic)
            fi.SetValue(Me, b)

        End Sub

        'Protected Overrides Sub RaiseItemClick(e As LabelInfoItemClickEventArgs)
        '    If e.InfoText.Tag Is Nothing Then
        '        MyBase.RaiseItemClick(e)
        '        Return
        '    Else

        '    End If
        '    If TypeOf e.InfoText.Tag Is NodeActionElementEx Then
        '        Dim elm As NodeActionElementEx = DirectCast(e.InfoText.Tag, NodeActionElementEx)
        '        'Dim menu As New DXPopupMenu()
        '        'Dim popup As New DXPopupStandardMenu(menu)


        '        'Dim a As New PopupMenuShowingEventArgs(elm.NodeEx, ElementType.Value, FilterControlMenuType.Column, menu, Me.MousePosition)
        '        'RaisePopupMenuShowing(a)
        '        'Me.ShowElementMenu(ElementType.Property)
        '        MyBase.RaiseItemClick(e)
        '        Return

        '    End If

        '    If TypeOf e.InfoText.Tag Is NodeParentheseElementEx Then
        '        Dim parentheseElement As NodeParentheseElementEx = DirectCast(e.InfoText.Tag, NodeParentheseElementEx)
        '        Dim labelInfo As FilterControlLabelInfoEx = DirectCast(Me.Model(parentheseElement.Node), FilterControlLabelInfoEx)
        '        labelInfo.ActivateParenthese(parentheseElement)
        '        Me.Refresh()
        '        Return
        '    End If
        '    If TypeOf e.InfoText.Tag Is NodeEditableElementEx Then
        '        Dim elm As NodeEditableElementEx = DirectCast(e.InfoText.Tag, NodeEditableElementEx)
        '        If elm.NodeEx.IsCustomFunction Then
        '            'MyBase.RaiseItemClick(e)
        '            Return

        '        End If
        '        Dim labelInfo As FilterControlLabelInfoEx = DirectCast(Me.Model(elm.Node), FilterControlLabelInfoEx)
        '        labelInfo.ResetParentheses()
        '        Me.Refresh()
        '        Dim nodeEditableElement As NodeEditableElementEx = DirectCast(e.InfoText.Tag, NodeEditableElementEx)
        '        If nodeEditableElement.ElementType.value__ = FieldActionEx.ShowFunctions.value__ Then
        '            If labelInfo.ViewInfo.Count > nodeEditableElement.Index Then
        '                Dim vis As New List(Of FilterLabelInfoTextViewInfo)
        '                Dim lvi As FilterLabelInfoTextViewInfo = Nothing
        '                For i As Integer = 0 To labelInfo.ViewInfo.Count - 1
        '                    Dim l As FilterLabelInfoTextViewInfo = TryCast(labelInfo.ViewInfo.Item(i), FilterLabelInfoTextViewInfo)
        '                    If l.InfoText.Tag Is nodeEditableElement Then
        '                        lvi = l
        '                        Exit For
        '                    End If
        '                Next
        '                If lvi IsNot Nothing Then
        '                    Dim arg As New PopupCustomFunctionsMenuShowingEventArgs(nodeEditableElement.Node, New Point(lvi.ItemBounds.X, lvi.ItemBounds.Y + lvi.ItemBounds.Height))
        '                    Me.RaiseCustomFunctionsPopupMenuShowing(arg)
        '                End If
        '            End If
        '        End If
        '    End If
        '    MyBase.RaiseItemClick(e)
        'End Sub

        'Protected Overrides Sub RaisePopupMenuShowing(ByVal e As PopupMenuShowingEventArgs)
        '    If e.MenuType = FilterControlMenuType.Clause Then
        '        e.Menu.Items.Add(New DXMenuItem("Matches any of", AddressOf OnClauseClick) With {.Tag = ClauseTypeEnumHelper.MatchesAnyOf})
        '        Dim clauseNode As ClauseNodeEx = TryCast(e.CurrentNode, ClauseNodeEx)
        '        Dim type As Type = clauseNode.GetFirstOperandType() 'clauseNode.FirstOperand
        '        For Each kv In XpoFunctionFilterManager.FunctionsFilterInfos
        '            If kv.Value.FirstParamType Is type AndAlso kv.Value.ResultType Is GetType(Boolean) Then
        '                e.Menu.Items.Add(New DXMenuItem(kv.Value.Display, AddressOf OnClauseClick) With {.Tag = kv.Value.Id})
        '            End If
        '        Next
        '    ElseIf e.MenuType = FilterControlMenuType.ColumnFunctions
        '        Dim clauseNode As ClauseNodeEx = TryCast(e.CurrentNode, ClauseNodeEx)
        '        If clauseNode IsNot Nothing Then
        '            If clauseNode.IsCustomFunction Then
        '                Dim functionParamererFilterInfo As XpoFunctionParamererFilterInfo = clauseNode.GetFocusedCustomFunctionParameterInfo
        '                Dim parameterType As Type = functionParamererFilterInfo?.ParameterInfo?.ParameterType
        '                If parameterType IsNot Nothing Then
        '                    Dim lst As List(Of DXMenuItem) = e.Menu.Items.OfType(Of DXMenuItem).Where(Function(q) q.Tag IsNot Nothing AndAlso TypeOf q.Tag Is DevExpress.XtraGrid.FilterEditor.GridFilterColumn AndAlso Not DirectCast(q.Tag, DevExpress.XtraGrid.FilterEditor.GridFilterColumn).ColumnType.IsCastableTo(parameterType)).ToList
        '                    For Each mi In lst
        '                        e.Menu.Items.Remove(mi)
        '                    Next
        '                    If parameterType IsNot GetType(Date) Then
        '                        Dim lmi As DXSubMenuItem = e.Menu.Items.OfType(Of DXSubMenuItem).LastOrDefault
        '                        If lmi IsNot Nothing Then
        '                            e.Menu.Items.Remove(lmi)
        '                        End If
        '                    End If
        '                End If
        '            End If
        '        End If

        '    End If
        '    MyBase.RaisePopupMenuShowing(e)
        'End Sub

        'Protected Overrides Sub RaiseBeforeShowValueEditor(e As ShowValueEditorEventArgs)
        '    MyBase.RaiseBeforeShowValueEditor(e)
        '    Dim elm As NodeEditableElementEx = DirectCast(DirectCast(e.CurrentNode, ClauseNodeEx).Elements(e.FocusedElementIndex), NodeEditableElementEx)
        '    Dim ov As OperandValue = TryCast(elm.Criteria, OperandValue)
        '    If ov IsNot Nothing AndAlso ov.Value IsNot Nothing AndAlso ov.Value.ToString <> "?" Then
        '        e.Editor.EditValue = ov.Value
        '    End If
        'End Sub




        'Protected Overrides Function GetRepositoryItem(node As ClauseNode) As RepositoryItem

        '    Dim nodeEx As ClauseNodeEx = DirectCast(node, ClauseNodeEx)
        '    Dim index As Integer = nodeEx.FocusInfoEx.ElementIndex
        '    Dim element As NodeEditableElementEx = nodeEx.GetFocusedElement()
        '    If element.IsParameter Then
        '        If element.ParameterOf IsNot Nothing AndAlso TypeOf element.ParameterOf Is NodeMathElementEx Then
        '            Return New RepositoryItemSpinEdit
        '        Else
        '            Return XNullable(Of RepositoryItem).Instance(element.GetParameterEditor()).IfNull(MyBase.GetRepositoryItem(node))
        '        End If
        '    End If
        '    If nodeEx.Operation.IsDefined Then
        '        Return MyBase.GetRepositoryItem(node)
        '    Else
        '        Dim filterInfos As XpoFunctionFilterInfo = XpoFunctionFilterManager.GetInfos(nodeEx.Operation.IntegerValue)
        '        If filterInfos Is Nothing Then
        '            Return MyBase.GetRepositoryItem(node)
        '        ElseIf filterInfos IsNot Nothing Then
        '            Dim editorIndex As Integer = Convert.ToInt32(nodeEx.Elements.LongCount(Function(q) (q.ElementType = ElementType.Value OrElse (q.IsValueElement AndAlso Not q.Text = "@#")) AndAlso q.Index <= index))
        '            Dim repositoryItem As RepositoryItem = filterInfos.GetFilterParameterEditor(editorIndex)
        '            If repositoryItem IsNot Nothing Then

        '                Return repositoryItem
        '            End If
        '        End If
        '    End If
        '    Return MyBase.GetRepositoryItem(node)
        'End Function
    End Class

End Namespace
