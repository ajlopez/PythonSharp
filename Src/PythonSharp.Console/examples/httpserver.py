
from System import Array, Byte
from System.Net import HttpListener
from System.IO import Path, FileStream, FileMode, File

root = "c:/apache-tomcat-6.0.18/webapps/docs"

bytes = Array.CreateInstance(Byte,1024)

listener = HttpListener()
listener.Prefixes.Add("http://*:8000/")

def process(context):
    filename = context.Request.Url.AbsolutePath
    if not filename or filename == '/':
        filename = "index.html"
    if filename[0] == '/':
        filename = filename[1:]
    print(filename)
    filename = Path.Combine(root, filename)
    print(filename)
    if not File.Exists(filename):
        context.Response.Abort()
        return
    input = FileStream(filename, FileMode.Open)
    bytes = Array.CreateInstance(Byte, 1024 * 16)
    nbytes = input.Read(bytes, 0, bytes.Length)
    while nbytes>0:
        context.Response.OutputStream.Write(bytes, 0, nbytes)
        nbytes = input.Read(bytes, 0, bytes.Length)
    input.Close()
    context.Response.OutputStream.Close()

listener.Start()

while True:
    context = listener.GetContext()
    print("new request")
    process(context)
        