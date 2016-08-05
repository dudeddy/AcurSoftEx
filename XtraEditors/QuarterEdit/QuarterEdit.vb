Imports System.ComponentModel
Imports AcurSoft.XtraEditors.Popup
Imports AcurSoft.XtraEditors.Repository
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing.Helpers
Imports DevExpress.XtraEditors

Namespace AcurSoft.XtraEditors

    Public Class QuarterEdit
        Inherits LookUpEdit

        'The static constructor which calls the registration method
        Shared Sub New()
            RepositoryItemQuarterEdit.RegisterEdit()
        End Sub

        'Initialize the new instance
        Public Sub New()
            '...
            'MyBase.EditValue = Date.Today.GetQuarter
        End Sub

        'Return the unique name
        Public Overrides ReadOnly Property EditorTypeName() As String
            Get
                Return RepositoryItemQuarterEdit.CustomEditName
            End Get
        End Property

        'Override the Properties property
        'Simply type-cast the object to the custom repository item type
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
        Public Shadows ReadOnly Property Properties() As RepositoryItemQuarterEdit
            Get
                Return TryCast(MyBase.Properties, RepositoryItemQuarterEdit)
            End Get
        End Property

    End Class
End Namespace
