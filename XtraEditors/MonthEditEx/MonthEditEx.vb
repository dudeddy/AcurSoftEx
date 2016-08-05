Imports System.ComponentModel
Imports AcurSoft.XtraEditors.Popup
Imports AcurSoft.XtraEditors.Repository
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing.Helpers
Imports DevExpress.XtraEditors

Namespace AcurSoft.XtraEditors

    Public Class MonthEditEx
        Inherits LookUpEdit

        'The static constructor which calls the registration method
        Shared Sub New()
            RepositoryItemMonthEditEx.RegisterEdit()
        End Sub

        'Initialize the new instance
        Public Sub New()
            '...
            'MyBase.EditValue = Date.Today.Month
        End Sub

        'Return the unique name
        Public Overrides ReadOnly Property EditorTypeName() As String
            Get
                Return RepositoryItemMonthEditEx.CustomEditName
            End Get
        End Property

        'Override the Properties property
        'Simply type-cast the object to the custom repository item type
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
        Public Shadows ReadOnly Property Properties() As RepositoryItemMonthEditEx
            Get
                Return TryCast(MyBase.Properties, RepositoryItemMonthEditEx)
            End Get
        End Property

        'Protected Friend Overridable Function IsVistaDisplayMode() As Boolean
        '    Return True
        'End Function

        'Protected Overrides Sub UpdateDisplayText()
        '    MyBase.UpdateDisplayText()
        'End Sub


        'Protected Overrides Function CreatePopupForm() As DevExpress.XtraEditors.Popup.PopupBaseForm
        '    Return New YearVistaPopupDateEditForm(Me)
        '    'Return New CustomPopupDateEditForm(Me)
        'End Function

        'Public Sub SetOldValue(value As Object)
        '    _OldEditValue = value
        'End Sub
        'Private _OldEditValue As Object
        'Public Overrides ReadOnly Property OldEditValue As Object
        '    Get
        '        Return _OldEditValue
        '    End Get
        'End Property

        'Private _Year As Integer
        'Public Property Year As Integer
        '    Get
        '        FlushPendingEditActions()
        '        If _Year = 0 Then
        '            _Year = Date.Today.Year
        '        End If
        '        Return _Year
        '        'Return ConvertToYear(Me.EditValue)
        '    End Get
        '    Set(value As Integer)
        '        Me.EditValue = value
        '    End Set
        'End Property

        'Public Function ConvertToYear(value As Object) As Integer
        '    Dim year As Integer = 0
        '    If value Is Nothing Then
        '        year = Date.Today.Year
        '    ElseIf TypeOf value Is Date Then
        '        year = DirectCast(value, Date).Year
        '        If year = 1 Then
        '            year = _Year
        '        End If
        '        'Return year
        '    ElseIf TypeOf value Is Integer Then
        '        year = DirectCast(value, Integer)
        '    Else
        '        Try
        '            year = DirectCast(value, Integer)
        '        Catch ex As Exception
        '            year = Date.Today.Year
        '        End Try
        '    End If
        '    If year = 0 Then
        '        year = 1
        '    End If
        '    Return year
        'End Function

        'Public Shadows Property DateTime As Date

        'Private _EditValue As Object
        'Public Overrides Property EditValue As Object
        '    Get
        '        'If _EditValue Is Nothing Then
        '        '    _EditValue = Date.Today.Year
        '        '    _Year = Date.Today.Year
        '        'End If
        '        Return MyBase.EditValue
        '    End Get
        '    Set(value As Object)
        '        'If value Is Nothing Then
        '        '    _EditValue = Date.Today.Year
        '        'ElseIf TypeOf value Is Date Then
        '        '    _EditValue = DirectCast(value, Date).Year
        '        'ElseIf TypeOf value Is Integer Then
        '        '    _EditValue = DirectCast(value, Integer)
        '        'Else
        '        '    _EditValue = Date.Today.Year
        '        'End If
        '        _Year = ConvertToYear(value)
        '        Me.DateTime = New Date(_Year, 1, 1)

        '        MyBase.EditValue = _Year
        '        'If value Is Nothing OrElse TypeOf value Is CriteriaOperator Then
        '        '    Me.Criteria = DirectCast(value, CriteriaOperator)
        '        'Else
        '        '    Me.Criteria = Nothing
        '        'End If
        '        'MyBase.EditValue = Me.Criteria
        '        RaiseEditValueChanged()
        '    End Set
        'End Property

        'Public Overrides Sub ShowPopup()
        '    MyBase.ShowPopup()
        '    If Me.PopupForm Is Nothing Then Return
        '    DirectCast(Me.PopupForm, YearVistaPopupDateEditForm).Calendar.DateTime = New Date(Me.Year, 1, 1)
        'End Sub

    End Class
End Namespace
