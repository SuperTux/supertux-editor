using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: AssemblyVersion("0.2")]
[assembly: AssemblyTitle("supertux-editor")]
[assembly: ComVisibleAttribute(false)]
[assembly: CLSCompliant(false)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum,
                              Execution = true, UnmanagedCode = true, Unrestricted = true)]
