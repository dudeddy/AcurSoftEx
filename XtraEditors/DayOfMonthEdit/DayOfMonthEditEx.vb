Imports System.ComponentModel
Imports AcurSoft.XtraEditors.Popup
Imports AcurSoft.XtraEditors.Repository
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing.Helpers
Imports DevExpress.XtraEditors

Namespace AcurSoft.XtraEditors

    Public Class DayOfMonthEditEx
        Inherits LookUpEdit

        'The static constructor which calls the registration method
        Shared Sub New()
            RepositoryItemDayOfMonthEditEx.RegisterEdit()
        End Sub

        'Initialize the new instance
        Public Sub New()
            '...
            'MyBase.EditValue = Date.Today.Day
        End Sub

        'Return the unique name
        Public Overrides ReadOnly Property EditorTypeName() As String
            Get
                Return RepositoryItemDayOfMonthEditEx.CustomEditName
            End Get
        End Property

        'Override the Properties property
        'Simply type-cast the object to the custom repository item type
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
        Public Shadows ReadOnly Property Properties() As RepositoryItemDayOfMonthEditEx
            Get
                Return TryCast(MyBase.Properties, RepositoryItemDayOfMonthEditEx)
            End Get
        End Property
    End Class
End Namespace
