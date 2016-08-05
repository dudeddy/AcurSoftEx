Imports System.ComponentModel
Imports DevExpress.Data.Filtering
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.ViewInfo

<UserRepositoryItem("RegisterDateCriteriaEdit")>
Public Class RepositoryItemDateCriteriaEdit
    Inherits RepositoryItemDateEdit
    Public Property FunctionOperatorType As FunctionOperatorType

    'Private _Functions As List(Of Object)

    'The static constructor which calls the registration method
    Shared Sub New()
        RegisterDateCriteriaEdit()
        'Dim x As FunctionOperator
        'x.OperatorType = FunctionOperatorType.IsOutlookIntervalToday
    End Sub


    'The unique name for the custom editor
    Public Const CustomEditName As String = "DateCriteriaEdit"

    'Return the unique name
    Public Overrides ReadOnly Property EditorTypeName() As String
        Get
            Return CustomEditName
        End Get
    End Property

    'Register the editor
    Public Shared Sub RegisterDateCriteriaEdit()
        'Icon representing the editor within a container editor's Designer
        Dim img As Image = Nothing
        'Try
        '    img = CType(Bitmap.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("DevExpress.CustomEditors.CustomEdit.bmp")), Bitmap)
        'Catch
        'End Try
        EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(CustomEditName, GetType(DateCriteriaEdit), GetType(RepositoryItemDateCriteriaEdit), GetType(DateEditViewInfo), New ButtonEditPainter(), True, img))
    End Sub

    'Override the Assign method
    Public Overrides Sub Assign(ByVal item As RepositoryItem)
        BeginUpdate()
        Try
            MyBase.Assign(item)
            Dim source As RepositoryItemDateCriteriaEdit = TryCast(item, RepositoryItemDateCriteriaEdit)
            If source Is Nothing Then Return
        Finally
            EndUpdate()
        End Try
    End Sub

    Public Overrides Property TextEditStyle As TextEditStyles
        Get
            Return TextEditStyles.DisableTextEditor
        End Get
        Set(value As TextEditStyles)
            'MyBase.TextEditStyle = value
        End Set
    End Property

    Public Overrides Function GetDisplayText(editValue As Object) As String
        If Me.Criteria IsNot Nothing AndAlso TypeOf Me.Criteria Is CriteriaOperator Then
            Return DirectCast(editValue, CriteriaOperator).ToString
        End If
        Return String.Empty
    End Function

    'Protected Overrides Sub RaiseCustomDisplayText(e As CustomDisplayTextEventArgs)
    '    MyBase.RaiseCustomDisplayText(e)
    '    Dim xx = e.Value
    '    Dim pp = Me.Criteria
    '    'If Me.Criteria IsNot Nothing AndAlso TypeOf Me.Criteria Is CriteriaOperator Then
    '    '        Return DirectCast(editValue, CriteriaOperator).ToString
    '    '    End If
    'End Sub

    Protected Overrides Sub RaiseEditValueChanged(e As EventArgs)
        Dim editor As DateCriteriaEdit = DirectCast(Me.OwnerEdit, DateCriteriaEdit)

        MyBase.RaiseEditValueChanged(e)
        editor.SetOldValue(editor.Criteria)
    End Sub

    Public Property EditIniMode As EditInitModeEnum = EditInitModeEnum.Values

    Private _EditMode As EditModeEnum = EditModeEnum.Normal
    Public Property EditMode As EditModeEnum
        Get
            Return _EditMode
        End Get
        Set(value As EditModeEnum)
            Select Case value
                Case EditModeEnum.Between
                    Me.ConfigButton.Image = Ressources.criteria_operator_between_16x16
                Case EditModeEnum.Normal
                    Me.ConfigButton.Image = Ressources.date_16x16
                Case EditModeEnum.Greater
                    Me.ConfigButton.Image = Ressources.criteria_operator_gt_16x16
                Case EditModeEnum.GreaterOrEqual
                    Me.ConfigButton.Image = Ressources.criteria_operator_ge_16x16
                Case EditModeEnum.Lesser
                    Me.ConfigButton.Image = Ressources.criteria_operator_lt_16x16
                Case EditModeEnum.LessOrEqual
                    Me.ConfigButton.Image = Ressources.criteria_operator_le_16x16
                Case EditModeEnum.Function
                    Me.ConfigButton.Image = Ressources.functions_16x16

            End Select
            _EditMode = value
        End Set
    End Property
    Public Property CriteriaPropertyName As String = "CriteriaPropertyName"

    Private _Criteria As CriteriaOperator
    Public Property Criteria As CriteriaOperator
        Get
            Return _Criteria
        End Get
        Set(value As CriteriaOperator)
            If _Criteria Is value Then Return
            _Criteria = value
        End Set
    End Property

    Protected Overrides Sub RaiseQueryCloseUp(e As CancelEventArgs)
        Dim edit As DateCriteriaEdit = DirectCast(Me.OwnerEdit, DateCriteriaEdit)
        If Me.EditMode = EditModeEnum.Between AndAlso edit.CriteriaPopUp IsNot Nothing AndAlso Not edit.CriteriaPopUp.OkButtonClicked Then
            e.Cancel = True
        End If
        MyBase.RaiseQueryCloseUp(e)
    End Sub

    Protected Overrides Sub RaiseQueryPopUp(e As CancelEventArgs)
        MyBase.RaiseQueryPopUp(e)
        Dim edit As DateCriteriaEdit = DirectCast(Me.OwnerEdit, DateCriteriaEdit)
        If Me.EditMode = EditModeEnum.Between AndAlso edit.CriteriaPopUp IsNot Nothing Then
            edit.CriteriaPopUp.Criteria = edit.Criteria
        End If
    End Sub

    Public ReadOnly Property ConfigButton As EditorButton

    Public Overrides Sub CreateDefaultButton()
        MyBase.CreateDefaultButton()
        _ConfigButton = New EditorButton With {.Tag = "config", .Kind = ButtonPredefines.Glyph, .ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter, .IsLeft = True, .Image = Ressources.cog_edit_16x16}
        Me.Buttons.Add(_ConfigButton)
    End Sub

    Protected Overrides Sub RaiseButtonClick(e As ButtonPressedEventArgs)
        MyBase.RaiseButtonClick(e)
        If e.Button IsNot Me.ConfigButton Then Return
        Dim bvi As EditorButtonObjectInfoArgs = DirectCast(Me.OwnerEdit.GetViewInfo(), ButtonEditViewInfo).ButtonInfoByButton(e.Button)
        Me.CreateConfigMenu.Show(Me.OwnerEdit, New Point(bvi.Bounds.Left, bvi.Bounds.Bottom))

    End Sub


    Public Overridable Function CreateConfigMenu() As DXPopupStandardMenu
        Dim _ConfigMenu As New DXPopupStandardMenu(New DXPopupMenu)
        'Dim mi As New DXMenuCheckItem() With {
        '    .Caption = "Selections de dates", .Tag = EditModeEnum.Normal, .Checked = (Me.EditMode = EditModeEnum.Normal)}
        Dim mi As New DXMenuItem("Selections de dates") With {.Tag = EditModeEnum.Normal, .Image = Ressources.date_16x16}
        _ConfigMenu.Menu.Items.Add(mi)
        AddHandler mi.Click, AddressOf ConfigMenuItemClick

        mi = New DXMenuItem("> Greater then") With {.Tag = EditModeEnum.Greater, .Image = Ressources.criteria_operator_gt_16x16}
        _ConfigMenu.Menu.Items.Add(mi)
        AddHandler mi.Click, AddressOf ConfigMenuItemClick
        mi = New DXMenuItem(">= Greater or equal ") With {.Tag = EditModeEnum.GreaterOrEqual, .Image = Ressources.criteria_operator_ge_16x16}
        _ConfigMenu.Menu.Items.Add(mi)
        AddHandler mi.Click, AddressOf ConfigMenuItemClick

        mi = New DXMenuItem("< Less then") With {.Tag = EditModeEnum.Lesser, .Image = Ressources.criteria_operator_lt_16x16}
        _ConfigMenu.Menu.Items.Add(mi)
        AddHandler mi.Click, AddressOf ConfigMenuItemClick
        mi = New DXMenuItem("<= Less or Equal") With {.Tag = EditModeEnum.LessOrEqual, .Image = Ressources.criteria_operator_le_16x16}
        _ConfigMenu.Menu.Items.Add(mi)
        AddHandler mi.Click, AddressOf ConfigMenuItemClick

        mi = New DXMenuItem("Between") With {.Tag = EditModeEnum.Between, .Image = Ressources.criteria_operator_between_16x16}
        _ConfigMenu.Menu.Items.Add(mi)
        AddHandler mi.Click, AddressOf ConfigMenuItemClick

        mi = New DXMenuItem("Function") With {.Tag = EditModeEnum.Function, .Image = Ressources.functions_16x16}
        _ConfigMenu.Menu.Items.Add(mi)
        AddHandler mi.Click, AddressOf ConfigMenuItemClick


        Return _ConfigMenu
    End Function

    Private Sub ConfigMenuItemClick(sender As Object, e As EventArgs)
        Dim mi As DXMenuItem = DirectCast(sender, DXMenuItem)
        Dim editMode As EditModeEnum = DirectCast(mi.Tag, EditModeEnum)

        Me.EditMode = editMode
        Me.ConfigButton.Image = mi.Image
        Me.Criteria = Nothing
        Me.OwnerEdit.ShowPopup()
    End Sub
End Class


