Namespace AcurSoft.XtraGrid.Views.Grid.Summary.Forms


    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ColumnSummariesForm
        Inherits DevExpress.XtraEditors.XtraForm

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.GridControl = New DevExpress.XtraGrid.GridControl()
            Me.GridView = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.GridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.redColumn = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
            Me.GridColumn2 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.redSummaryType = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
            Me.GridColumn3 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.redSortOrder = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
            Me.GridColumn4 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
            Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
            Me.btnOk = New DevExpress.XtraEditors.SimpleButton()
            Me.btnAdd = New DevExpress.XtraEditors.SimpleButton()
            Me.btnDown = New DevExpress.XtraEditors.SimpleButton()
            Me.btnRemove = New DevExpress.XtraEditors.SimpleButton()
            Me.btnUp = New DevExpress.XtraEditors.SimpleButton()
            Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.EmptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.GridColumn5 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.redTop = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
            CType(Me.GridControl, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.redColumn, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.redSummaryType, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.redSortOrder, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.LayoutControl1.SuspendLayout()
            CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.redTop, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'GridControl
            '
            Me.GridControl.Location = New System.Drawing.Point(4, 30)
            Me.GridControl.MainView = Me.GridView
            Me.GridControl.Name = "GridControl"
            Me.GridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.redSummaryType, Me.redSortOrder, Me.redColumn, Me.redTop})
            Me.GridControl.Size = New System.Drawing.Size(667, 220)
            Me.GridControl.TabIndex = 0
            Me.GridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView})
            '
            'GridView
            '
            Me.GridView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn1, Me.GridColumn2, Me.GridColumn3, Me.GridColumn4, Me.GridColumn5})
            Me.GridView.GridControl = Me.GridControl
            Me.GridView.Name = "GridView"
            Me.GridView.OptionsView.ShowGroupPanel = False
            Me.GridView.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.GridColumn4, DevExpress.Data.ColumnSortOrder.Ascending)})
            '
            'GridColumn1
            '
            Me.GridColumn1.Caption = "Column"
            Me.GridColumn1.ColumnEdit = Me.redColumn
            Me.GridColumn1.FieldName = "Col"
            Me.GridColumn1.Name = "GridColumn1"
            Me.GridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
            Me.GridColumn1.Visible = True
            Me.GridColumn1.VisibleIndex = 0
            Me.GridColumn1.Width = 141
            '
            'redColumn
            '
            Me.redColumn.AutoHeight = False
            Me.redColumn.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.redColumn.Name = "redColumn"
            '
            'GridColumn2
            '
            Me.GridColumn2.Caption = "Type"
            Me.GridColumn2.ColumnEdit = Me.redSummaryType
            Me.GridColumn2.FieldName = "SummaryType"
            Me.GridColumn2.Name = "GridColumn2"
            Me.GridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
            Me.GridColumn2.Visible = True
            Me.GridColumn2.VisibleIndex = 1
            Me.GridColumn2.Width = 143
            '
            'redSummaryType
            '
            Me.redSummaryType.AutoHeight = False
            Me.redSummaryType.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.redSummaryType.Name = "redSummaryType"
            '
            'GridColumn3
            '
            Me.GridColumn3.Caption = "Display Format"
            Me.GridColumn3.FieldName = "DisplayFormat"
            Me.GridColumn3.Name = "GridColumn3"
            Me.GridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
            Me.GridColumn3.Visible = True
            Me.GridColumn3.VisibleIndex = 2
            Me.GridColumn3.Width = 193
            '
            'redSortOrder
            '
            Me.redSortOrder.AutoHeight = False
            Me.redSortOrder.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.redSortOrder.Name = "redSortOrder"
            '
            'GridColumn4
            '
            Me.GridColumn4.Caption = "Order"
            Me.GridColumn4.FieldName = "Order"
            Me.GridColumn4.Name = "GridColumn4"
            Me.GridColumn4.OptionsColumn.AllowEdit = False
            Me.GridColumn4.Visible = True
            Me.GridColumn4.VisibleIndex = 4
            Me.GridColumn4.Width = 93
            '
            'LayoutControl1
            '
            Me.LayoutControl1.AllowCustomization = False
            Me.LayoutControl1.Controls.Add(Me.btnCancel)
            Me.LayoutControl1.Controls.Add(Me.btnOk)
            Me.LayoutControl1.Controls.Add(Me.GridControl)
            Me.LayoutControl1.Controls.Add(Me.btnAdd)
            Me.LayoutControl1.Controls.Add(Me.btnDown)
            Me.LayoutControl1.Controls.Add(Me.btnRemove)
            Me.LayoutControl1.Controls.Add(Me.btnUp)
            Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LayoutControl1.HiddenItems.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
            Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControl1.Name = "LayoutControl1"
            Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(225, 301, 250, 350)
            Me.LayoutControl1.Root = Me.LayoutControlGroup1
            Me.LayoutControl1.Size = New System.Drawing.Size(675, 280)
            Me.LayoutControl1.TabIndex = 1
            Me.LayoutControl1.Text = "LayoutControl1"
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(565, 254)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(106, 22)
            Me.btnCancel.StyleController = Me.LayoutControl1
            Me.btnCancel.TabIndex = 9
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(455, 254)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(106, 22)
            Me.btnOk.StyleController = Me.LayoutControl1
            Me.btnOk.TabIndex = 8
            Me.btnOk.Text = "Ok"
            '
            'btnAdd
            '
            Me.btnAdd.Location = New System.Drawing.Point(4, 4)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(96, 22)
            Me.btnAdd.StyleController = Me.LayoutControl1
            Me.btnAdd.TabIndex = 7
            Me.btnAdd.Text = "Add"
            '
            'btnDown
            '
            Me.btnDown.Location = New System.Drawing.Point(535, 4)
            Me.btnDown.Name = "btnDown"
            Me.btnDown.Size = New System.Drawing.Size(66, 22)
            Me.btnDown.StyleController = Me.LayoutControl1
            Me.btnDown.TabIndex = 6
            Me.btnDown.Text = "Down"
            '
            'btnRemove
            '
            Me.btnRemove.Location = New System.Drawing.Point(104, 4)
            Me.btnRemove.Name = "btnRemove"
            Me.btnRemove.Size = New System.Drawing.Size(96, 22)
            Me.btnRemove.StyleController = Me.LayoutControl1
            Me.btnRemove.TabIndex = 5
            Me.btnRemove.Text = "Remove"
            '
            'btnUp
            '
            Me.btnUp.Location = New System.Drawing.Point(605, 4)
            Me.btnUp.Name = "btnUp"
            Me.btnUp.Size = New System.Drawing.Size(66, 22)
            Me.btnUp.StyleController = Me.LayoutControl1
            Me.btnUp.TabIndex = 4
            Me.btnUp.Text = "Up"
            '
            'LayoutControlItem1
            '
            Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 26)
            Me.LayoutControlItem1.Name = "LayoutControlItem1"
            Me.LayoutControlItem1.Size = New System.Drawing.Size(451, 30)
            Me.LayoutControlItem1.TextSize = New System.Drawing.Size(50, 20)
            '
            'LayoutControlGroup1
            '
            Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
            Me.LayoutControlGroup1.GroupBordersVisible = False
            Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem6, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.EmptySpaceItem1, Me.LayoutControlItem7, Me.EmptySpaceItem2, Me.LayoutControlItem8})
            Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControlGroup1.Name = "Root"
            Me.LayoutControlGroup1.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
            Me.LayoutControlGroup1.Size = New System.Drawing.Size(675, 280)
            Me.LayoutControlGroup1.TextVisible = False
            '
            'LayoutControlItem6
            '
            Me.LayoutControlItem6.Control = Me.GridControl
            Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 26)
            Me.LayoutControlItem6.Name = "LayoutControlItem6"
            Me.LayoutControlItem6.Size = New System.Drawing.Size(671, 224)
            Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem6.TextVisible = False
            '
            'LayoutControlItem2
            '
            Me.LayoutControlItem2.Control = Me.btnUp
            Me.LayoutControlItem2.Location = New System.Drawing.Point(601, 0)
            Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(70, 26)
            Me.LayoutControlItem2.MinSize = New System.Drawing.Size(70, 26)
            Me.LayoutControlItem2.Name = "LayoutControlItem2"
            Me.LayoutControlItem2.Size = New System.Drawing.Size(70, 26)
            Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem2.TextToControlDistance = 0
            Me.LayoutControlItem2.TextVisible = False
            '
            'LayoutControlItem3
            '
            Me.LayoutControlItem3.Control = Me.btnRemove
            Me.LayoutControlItem3.Location = New System.Drawing.Point(100, 0)
            Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(100, 0)
            Me.LayoutControlItem3.MinSize = New System.Drawing.Size(100, 26)
            Me.LayoutControlItem3.Name = "LayoutControlItem3"
            Me.LayoutControlItem3.Size = New System.Drawing.Size(100, 26)
            Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem3.TextToControlDistance = 0
            Me.LayoutControlItem3.TextVisible = False
            '
            'LayoutControlItem4
            '
            Me.LayoutControlItem4.Control = Me.btnDown
            Me.LayoutControlItem4.Location = New System.Drawing.Point(531, 0)
            Me.LayoutControlItem4.MaxSize = New System.Drawing.Size(70, 26)
            Me.LayoutControlItem4.MinSize = New System.Drawing.Size(70, 26)
            Me.LayoutControlItem4.Name = "LayoutControlItem4"
            Me.LayoutControlItem4.Size = New System.Drawing.Size(70, 26)
            Me.LayoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem4.TextToControlDistance = 0
            Me.LayoutControlItem4.TextVisible = False
            '
            'LayoutControlItem5
            '
            Me.LayoutControlItem5.Control = Me.btnAdd
            Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControlItem5.MaxSize = New System.Drawing.Size(100, 26)
            Me.LayoutControlItem5.MinSize = New System.Drawing.Size(100, 26)
            Me.LayoutControlItem5.Name = "LayoutControlItem5"
            Me.LayoutControlItem5.Size = New System.Drawing.Size(100, 26)
            Me.LayoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem5.TextToControlDistance = 0
            Me.LayoutControlItem5.TextVisible = False
            '
            'EmptySpaceItem1
            '
            Me.EmptySpaceItem1.AllowHotTrack = False
            Me.EmptySpaceItem1.Location = New System.Drawing.Point(200, 0)
            Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
            Me.EmptySpaceItem1.Size = New System.Drawing.Size(331, 26)
            Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
            '
            'LayoutControlItem7
            '
            Me.LayoutControlItem7.Control = Me.btnOk
            Me.LayoutControlItem7.Location = New System.Drawing.Point(451, 250)
            Me.LayoutControlItem7.MaxSize = New System.Drawing.Size(110, 26)
            Me.LayoutControlItem7.MinSize = New System.Drawing.Size(110, 26)
            Me.LayoutControlItem7.Name = "LayoutControlItem7"
            Me.LayoutControlItem7.Size = New System.Drawing.Size(110, 26)
            Me.LayoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem7.TextToControlDistance = 0
            Me.LayoutControlItem7.TextVisible = False
            '
            'EmptySpaceItem2
            '
            Me.EmptySpaceItem2.AllowHotTrack = False
            Me.EmptySpaceItem2.Location = New System.Drawing.Point(0, 250)
            Me.EmptySpaceItem2.Name = "EmptySpaceItem2"
            Me.EmptySpaceItem2.Size = New System.Drawing.Size(451, 26)
            Me.EmptySpaceItem2.TextSize = New System.Drawing.Size(0, 0)
            '
            'LayoutControlItem8
            '
            Me.LayoutControlItem8.Control = Me.btnCancel
            Me.LayoutControlItem8.Location = New System.Drawing.Point(561, 250)
            Me.LayoutControlItem8.MaxSize = New System.Drawing.Size(110, 26)
            Me.LayoutControlItem8.MinSize = New System.Drawing.Size(110, 26)
            Me.LayoutControlItem8.Name = "LayoutControlItem8"
            Me.LayoutControlItem8.Size = New System.Drawing.Size(110, 26)
            Me.LayoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItem8.TextToControlDistance = 0
            Me.LayoutControlItem8.TextVisible = False
            '
            'GridColumn5
            '
            Me.GridColumn5.Caption = "Info"
            Me.GridColumn5.FieldName = "Info"
            Me.GridColumn5.Name = "GridColumn5"
            Me.GridColumn5.Visible = True
            Me.GridColumn5.VisibleIndex = 3
            Me.GridColumn5.Width = 79
            '
            'redTop
            '
            Me.redTop.AutoHeight = False
            Me.redTop.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.redTop.IsFloatValue = False
            Me.redTop.Mask.EditMask = "N00"
            Me.redTop.MaxValue = New Decimal(New Integer() {1000000000, 0, 0, 0})
            Me.redTop.MinValue = New Decimal(New Integer() {1, 0, 0, 0})
            Me.redTop.Name = "redTop"
            '
            'ColumnSummariesForm
            '
            Me.AcceptButton = Me.btnOk
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(675, 280)
            Me.Controls.Add(Me.LayoutControl1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
            Me.Name = "ColumnSummariesForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Column Total Summaries"
            CType(Me.GridControl, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.redColumn, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.redSummaryType, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.redSortOrder, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.LayoutControl1.ResumeLayout(False)
            CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.redTop, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents GridControl As DevExpress.XtraGrid.GridControl
        Friend WithEvents GridView As DevExpress.XtraGrid.Views.Grid.GridView
        Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
        Friend WithEvents btnAdd As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents btnDown As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents btnRemove As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents btnUp As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
        Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
        Friend WithEvents GridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents GridColumn2 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents GridColumn3 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents GridColumn4 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents redSummaryType As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Friend WithEvents redSortOrder As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Friend WithEvents redColumn As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents EmptySpaceItem2 As DevExpress.XtraLayout.EmptySpaceItem
        Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents GridColumn5 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents redTop As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    End Class
End Namespace