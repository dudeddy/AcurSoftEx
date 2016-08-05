Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Popup

Namespace AcurSoft.XtraEditors.Popup
    Public Class YearVistaPopupDateEditForm
        Inherits VistaPopupDateEditForm

        Public Sub New(ByVal de As DateEdit)
            MyBase.New(de)
        End Sub
        Public Sub New(ByVal de As YearEdit)
            MyBase.New(de)
            'de.EditValue = 
        End Sub

        Public ReadOnly Property YearEdit As YearEdit
            Get
                Return DirectCast(Me.OwnerEdit, YearEdit)
            End Get
        End Property

        Protected Overrides Function CreateCalendar() As CalendarControl
            Dim yearVistaDateEditCalendar As New YearVistaDateEditCalendar(Me.YearEdit)
            If Me.YearEdit.EditValue Is Nothing Then
            ElseIf TypeOf Me.YearEdit.EditValue Is Date Then
                yearVistaDateEditCalendar.EditValue = New Date(Me.YearEdit.Year, 1, 1)
            ElseIf TypeOf Me.YearEdit.EditValue Is Integer Then
                yearVistaDateEditCalendar.EditValue = New Date(DirectCast(Me.YearEdit.EditValue, Integer), 1, 1)
            End If
            Return yearVistaDateEditCalendar
        End Function
    End Class
End Namespace
