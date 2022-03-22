using UnityEngine;
using UnityEditor;
using System;

namespace HK.Framework.Editor
{
	public abstract class EditorBase : UnityEditor.Editor
	{
		/// <summary>
		/// EditorGUILayoutのHorizontalのラッピング.
		/// </summary>
		/// <param name='func'>
		/// Func.
		/// </param>
		/// <param name='options'>
		/// Options.
		/// </param>
		protected void Horizontal( Action func, params GUILayoutOption[] options )
		{
			EditorGUILayout.BeginHorizontal( options );
			func();
			EditorGUILayout.EndHorizontal();
		}
		/// <summary>
		/// EditorGUILayoutのVerticalのラッピング.
		/// </summary>
		/// <param name='func'>
		/// Func.
		/// </param>
		/// <param name='options'>
		/// Options.
		/// </param>
		protected void Vertical( Action func, params GUILayoutOption[] options )
		{
			EditorGUILayout.BeginVertical( options );
			func();
			EditorGUILayout.EndVertical();
		}
		/// <summary>
		/// GUILayout.Buttonのラッピング.
		/// </summary>
		/// <param name='text'>
		/// Text.
		/// </param>
		/// <param name='func'>
		/// Func.
		/// </param>
		/// <param name='options'>
		/// Options.
		/// </param>
		protected void Button( string text, System.Action func, params GUILayoutOption[] options )
		{
			if( GUILayout.Button( text, options ) )
			{
				func();
			}
		}
		/// <summary>
		/// GUILayout.Labelのラッピング.
		/// </summary>
		/// <param name='text'>
		/// Text.
		/// </param>
		/// <param name='options'>
		/// Options.
		/// </param>
	    protected void Label( string text, params GUILayoutOption[] options )
	    {
	        GUILayout.Label( text, options );
	    }
	    protected void Label( GUIContent content, params GUILayoutOption[] options )
	    {
	        GUILayout.Label( content, options );
	    }
	    /// <summary>
		/// EditorGUILayout.Spaceのラッピング.
		/// </summary>
		protected void Space()
		{
			EditorGUILayout.Space();
		}
		/// <summary>
		/// GUILayout.Widthのラッピング.
		/// </summary>
		/// <param name='width'>
		/// Width.
		/// </param>
		protected GUILayoutOption Width( float width )
		{
			return GUILayout.Width( width );
		}
		/// <summary>
		/// EditorGUILayout.IntFieldのラッピング.
		/// </summary>
		/// <returns>
		/// The field.
		/// </returns>
		/// <param name='value'>
		/// Value.
		/// </param>
		/// <param name='options'>
		/// Options.
		/// </param>
		protected int IntField( int value, params GUILayoutOption[] options )
		{
			return EditorGUILayout.IntField( value, options );
		}
		/// <summary>
		/// EditorGUILayout.IntFieldラッピング.
		/// </summary>
		/// <returns>
		/// The field.
		/// </returns>
		/// <param name='label'>
		/// Label.
		/// </param>
		/// <param name='value'>
		/// Value.
		/// </param>
		/// <param name='options'>
		/// Options.
		/// </param>
		protected int IntField( string label, int value, params GUILayoutOption[] options )
		{
			return EditorGUILayout.IntField( label, value, options );
		}
		/// <summary>
		/// 囲いレイアウト付きで描画を行う.
		/// </summary>
		/// <param name='label'>
		/// Label.
		/// </param>
		/// <param name='func'>
		/// Func.
		/// </param>
		/// <param name='isDrawTopLine'>
		/// Is draw top line.
		/// </param>
		/// <param name='isDrawUnderLine'>
		/// Is draw under line.
		/// </param>
	    protected void Enclose( string label, System.Action func, bool isDrawTopLine = false, bool isDrawUnderLine = true )
	    {
	        this._Enclose( () => this.Label( label ), func, isDrawTopLine, isDrawUnderLine );
	    }
	    protected void Enclose( GUIContent labelContent, System.Action func, bool isDrawTopLine = false, bool isDrawUnderLine = true )
	    {
	        this._Enclose( () => this.Label( labelContent ), func, isDrawTopLine, isDrawUnderLine );
	    }

	    private void _Enclose( System.Action labelFunc, System.Action func, bool isDrawTopLine = false, bool isDrawUnderLine = true )
	    {
	        if( isDrawTopLine )
	        {
	            Line();
	        }
	        labelFunc();
	        EditorGUI.indentLevel++;
	        func();
	        EditorGUI.indentLevel--;
	        if( isDrawUnderLine )
	        {
	            Line();
	        }
	    }
		/// <summary>
		/// GUIに線を描画する.
		/// </summary>
		protected void Line()
		{
			GUILayout.Box( "", GUILayout.ExpandWidth( true ), GUILayout.Height( 2 ) );
		}
	}
}
