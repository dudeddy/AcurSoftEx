Imports AcurSoft.XtraEditors
Imports AcurSoftTests
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers

Public Class XtraForm2
    Private Sub XtraForm2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim editor As New YearEdit
        'Dim editor As New MonthEditEx
        Dim editor As New QuarterEdit
        Me.Controls.Add(editor)
        editor.BringToFront()
        AddHandler editor.EditValueChanged, Sub(s, a)
                                                'Me.Text = editor.EditValue & "  " & editor.Text
                                            End Sub
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim n = CriteriaToTreeProcessor.GetTree(New NodesFactoryAdv, CriteriaOperator.Parse("[test] = Thisyear()"), New List(Of CriteriaOperator))
        Dim zz = 1
    End Sub
End Class