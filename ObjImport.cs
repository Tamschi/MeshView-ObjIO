/*
 *  Copyright 2012 Tamme Schichler <tammeschichler@googlemail.com>
 * 
 *  This file is part of ObjImport.
 *
 *  ObjImport is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  ObjImport is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with ObjImport.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace ObjIO
{
    //WARNING: This is a .obj parsing function for MeshView. It isn't good code. Don't use it anywhere (unless you want a very slow and inefficient program).
    public static class ObjImport
    {
        public static void ParseFile(string path, out float[][] positions, out float[][] uvs, out float[][] normals, out int[][][] faces, IFormatProvider floatFormat = null)
        {
            floatFormat = floatFormat ?? CultureInfo.InvariantCulture;

            var lines = File.ReadAllLines(path);

            var pos = new LinkedList<float[]>();
            var uv = new LinkedList<float[]>();
            var n = new LinkedList<float[]>();
            var f = new LinkedList<int[][]>();

            var separators = new[] { ' ' };
            var faceSeparators = new[] { '/' };

            foreach (var line in lines)
            {
                var items = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length > 0)
                {
                    switch (items[0])
                    {
                        case "v":
                            pos.AddLast(items.Skip(1).Select(x => float.Parse(x, floatFormat)).ToArray());
                            break;

                        case "vt":
                            uv.AddLast(items.Skip(1).Select(x => float.Parse(x, floatFormat)).ToArray());
                            break;

                        case "vn":
                            n.AddLast(items.Skip(1).Select(x => float.Parse(x, floatFormat)).ToArray());
                            break;

                        case "f":
                            f.AddLast(items.Skip(1).Select(x => x.Split(faceSeparators, StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y, floatFormat) - 1).ToArray()).ToArray());
                            break;

                        default:
                            break;
                    }
                }
            }

            positions = pos.ToArray();
            uvs = uv.ToArray();
            normals = n.ToArray();
            faces = f.ToArray();
        }
    }
}
