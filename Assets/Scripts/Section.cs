using UnityEngine;
using System.Collections;

public class Section{

	public Vector2 pos,size;
	public Vector2 centerPos;
	public int nodeIndex, parent;

	public Section(float x=0, float y=0, float w=10, float h=10, int _nodeIndex=0, int _parent=0){
		pos.Set (x, y);
		size.Set (w, h);
		centerPos.Set (x+w/2f, y+h/2f);
		nodeIndex = _nodeIndex;
		parent = _parent;
	}

	public Section(Vector2 _pos, Vector2 _size, int _nodeIndex=0, int _parent=0){
		pos.Set(_pos.x,_pos.y);
		size.Set(_size.x, _size.y);
		centerPos.Set (_pos.x + (_size.x/2f), _pos.y + (_size.y/2f));
		nodeIndex = _nodeIndex;
		parent = _parent;
	}

	public Section(Section _s){
		pos.Set (_s.pos.x,_s.pos.y);
		size.Set (_s.size.x,_s.size.y);
		centerPos.Set (_s.centerPos.x, _s.centerPos.y);
		nodeIndex = _s.nodeIndex;
		parent = _s.parent;
	}

	public void printSection(){
		Debug.Log(string.Format("X: {0}, Y: {1}, Width: {2}, Height: {3}, Node Index: {4}, Node Parent: {5}, CenterX: {6}, CenterY: {7}",pos.x,pos.y,size.x,size.y,nodeIndex,parent,centerPos.x,centerPos.y));
	}
}
