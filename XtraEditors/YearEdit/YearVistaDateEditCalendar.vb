Imports AcurSoft.XtraEditors
Imports DevExpress.XtraEditors

Public Class YearVistaDateEditCalendar
    Inherits VistaCalendarControl

    Public ReadOnly Property YearEdit As YearEdit

    Public Sub New(yearEdit As YearEdit)
        MyBase.New()
        Me.YearEdit = yearEdit
    End Sub

    Protected Overrides Sub OnEditValueChanged()
        MyBase.OnEditValueChanged()
        'Me.YearEdit.Year = Me.DateTime.Year
    End Sub

    Public Sub New()
        MyBase.New()
        'Me.VistaCalendarViewStyle = VistaCalendarViewStyle.YearsGroupView Or VistaCalendarViewStyle.CenturyView

        'Me.AutoSize = False
        'btn = New SimpleButton()
        'AddHandler btn.Click, AddressOf OnNewButtonClick
        'btn.Text = "New Button"
        'Controls.Add(btn)
        'btn.Size = New Size(100, 30)
        'btn.Location = New Point(Bounds.X + Bounds.Width \ 2 - btn.Size.Width \ 2, Bounds.Bottom - indent)
    End Sub

    'Private btn As SimpleButton
    'Private indent As Integer = 5
    'Public ReadOnly Property Button() As SimpleButton
    '    Get
    '        Return btn
    '    End Get
    'End Property
    'Private Sub OnNewButtonClick(ByVal sender As Object, ByVal e As EventArgs)
    '    XtraMessageBox.Show("New Button Click")
    'End Sub
    'Public Overrides Function CalcBestSize() As Size
    '    If Button Is Nothing Then
    '        Return MyBase.CalcBestSize()
    '    End If
    '    Dim bestSize As Size = MyBase.CalcBestSize()
    '    bestSize.Height += Button.Height + indent
    '    Return bestSize
    'End Function
End Class
