Imports System.ComponentModel
Imports DevExpress.Data.Filtering
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing.Helpers
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Public Class DateCriteriaEdit
    Inherits DateEdit

    'The static constructor which calls the registration method
    Shared Sub New()
        RepositoryItemDateCriteriaEdit.RegisterDateCriteriaEdit()

    End Sub

    'Initialize the new instance
    Public Sub New()
        '...
        'If Not Me.DesignMode Then
        '    RemoveHandler Me.CustomDisplayText, AddressOf OnCustomDisplayText
        '    AddHandler Me.CustomDisplayText, AddressOf OnCustomDisplayText
        'End If
    End Sub

    Protected Overrides Sub UpdateDisplayText()
        If Me.Criteria Is Nothing Then Return
        MyBase.UpdateDisplayText()
    End Sub

    'Private Sub OnCustomDisplayText(sender As Object, e As CustomDisplayTextEventArgs)
    '    Dim x = e.Value

    'End Sub

    'Return the unique name
    Public Overrides ReadOnly Property EditorTypeName() As String
        Get
            Return RepositoryItemDateCriteriaEdit.CustomEditName
        End Get
    End Property

    'Override the Properties property
    'Simply type-cast the object to the custom repository item type
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public Shadows ReadOnly Property Properties() As RepositoryItemDateCriteriaEdit
        Get
            Return TryCast(MyBase.Properties, RepositoryItemDateCriteriaEdit)
        End Get
    End Property

    'Protected Friend Overridable Function IsVistaDisplayMode() As Boolean
    '    If Properties.VistaDisplayMode = DefaultBoolean.False Then
    '        Return False
    '    End If
    '    If Properties.VistaDisplayMode = DefaultBoolean.True Then
    '        Return True
    '    End If
    '    Return NativeVista.IsVista
    'End Function

    Public ReadOnly Property CriteriaPopUp As FilterPopupDateEditForm
    Public ReadOnly Property CriteriaFunctionsPopUp As FilterPopupDateEditFunctionsForm

    Protected Overrides Function CreatePopupForm() As DevExpress.XtraEditors.Popup.PopupBaseForm
        If Me.Properties.EditMode = EditModeEnum.Function Then
            _CriteriaFunctionsPopUp = New FilterPopupDateEditFunctionsForm(Me)
            Return _CriteriaFunctionsPopUp
        Else
            _CriteriaPopUp = New FilterPopupDateEditForm(Me)
            Return _CriteriaPopUp
        End If
    End Function



    Private _Criteria As CriteriaOperator
    Public Property Criteria As CriteriaOperator
        Get
            Return Me.Properties.Criteria
        End Get
        Set(value As CriteriaOperator)
            Me.Properties.Criteria = value
        End Set
    End Property



    'Protected Overrides Sub UpdateDisplayText()
    '    MyBase.UpdateDisplayText()
    'End Sub
    Public Sub SetOldValue(value As Object)
        _OldEditValue = value
    End Sub
    Private _OldEditValue As Object
    Public Overrides ReadOnly Property OldEditValue As Object
        Get
            Return _OldEditValue
        End Get
    End Property

    Public Overrides Property EditValue As Object
        Get
            Return Me.Criteria
        End Get
        Set(value As Object)
            If value Is Nothing OrElse TypeOf value Is CriteriaOperator Then
                Me.Criteria = DirectCast(value, CriteriaOperator)
            Else
                Me.Criteria = Nothing
            End If
            MyBase.EditValue = Me.Criteria
            RaiseEditValueChanged()
        End Set
    End Property

    Protected Overrides Sub OnPopupClosed(closeMode As PopupCloseMode)
        MyBase.OnPopupClosed(closeMode)
    End Sub


    Protected Overrides Sub OnPopupClosing(e As CloseUpEventArgs)
        If e.CloseMode = PopupCloseMode.Normal Then
            If Me.Properties.EditMode <> EditModeEnum.Function Then
                Dim popupForm As FilterPopupDateEditForm = DirectCast(Me.PopupForm, FilterPopupDateEditForm)
                Me.Criteria = popupForm.GetCriteria
            Else

                Dim popupForm As FilterPopupDateEditFunctionsForm = DirectCast(Me.PopupForm, FilterPopupDateEditFunctionsForm)
                Me.Criteria = popupForm.Criteria
            End If
        End If
        MyBase.OnPopupClosing(e)
    End Sub

    Protected Overrides Sub UpdateEditValueOnClose(closeMode As PopupCloseMode, acceptValue As Boolean, newValue As Object, oldValue As Object)
        If acceptValue And closeMode = PopupCloseMode.Normal Then
            'Dim criteria As CriteriaOperator = Nothing
            If Me.Properties.EditMode = EditModeEnum.Function Then
                MyBase.UpdateEditValueOnClose(closeMode, acceptValue, DirectCast(Me.PopupForm, FilterPopupDateEditFunctionsForm).Criteria, Me.OldEditValue)
            Else
                MyBase.UpdateEditValueOnClose(closeMode, acceptValue, DirectCast(Me.PopupForm, FilterPopupDateEditForm).GetCriteria, Me.OldEditValue)

            End If
        End If
    End Sub
End Class


