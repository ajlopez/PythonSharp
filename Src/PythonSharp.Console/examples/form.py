
from System.Windows.Forms import Form, Button, Application
from PythonSharp.Utilities import ObjectUtilities

f = Form()
f.Text = "PythonSharp Form"
b = Button()
b.Text = "Hello"
b.Width = 150
b.Height = 50
f.Controls.Add(b)

def click(sender, event):
   print("Click")

ObjectUtilities.AddHandler(b, "Click", click, locals())

Application.Run(f)

