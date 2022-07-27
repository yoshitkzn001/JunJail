using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncMove : MonoBehaviour
{
    public void AccelMoveX(int t, int maxt, float x1, float x2, GameObject obj)
    {
        int T = maxt - t;

        int startX = (int)x1;
        int targetX = (int)x2;
        float adjustX = x2 - targetX;
        int posX;
        if (T == 0)
        {
            posX = startX;
        }
        else
        {
            posX = (int)(((targetX - startX) / Mathf.Pow(maxt, 2)) * Mathf.Pow(T, 2) + startX);
        }

        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.x = posX + adjustX;
        transform.localPosition = pos;
    }
    public void AccelMoveY(int t, int maxt, float y1, float y2, GameObject obj)
    {
        int T = maxt - t;

        int startY = (int)y1;
        int targetY = (int)y2;
        float adjustY = y2 - targetY;
        int posY;
        if (T == 0)
        {
            posY = startY;
        }
        else
        {
            posY = (int)(((targetY - startY) / Mathf.Pow(maxt, 2)) * Mathf.Pow(T, 2) + startY);
        }

        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.y = posY + adjustY;
        transform.localPosition = pos;
    }

    public void DeccelMoveX(int t, int maxt, float x1, float x2, GameObject obj)
    {
        int startX = (int)x1;
        int targetX = (int)x2;
        float adjustX = x2 - targetX;
        int posX;
        if (t == 0)
        {
            posX = targetX;
        }
        else
        {
            posX = (int)(((startX - targetX) / Mathf.Pow(maxt, 2)) * Mathf.Pow(t, 2) + targetX);
        }

        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.x = posX + adjustX;
        transform.localPosition = pos;
    }
    public void DeccelMoveY(int t, int maxt, float y1, float y2, GameObject obj)
    {
        int startY = (int)y1;
        int targetY = (int)y2;
        float adjustY = y2 - targetY;
        int posY;
        if (t == 0)
        {
            posY = targetY;
        }
        else
        {
            posY = (int)(((startY - targetY) / Mathf.Pow(maxt, 2)) * Mathf.Pow(t, 2) + targetY);
        }

        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.y = posY + adjustY;
        transform.localPosition = pos;
    }

    public void SetSamePos(GameObject obj, GameObject targetobj, float plusX, float plusY, bool sameZ = false)
    {
        Transform targettransform = targetobj.transform;
        Vector3 targetpos = targettransform.position;
        Transform transform = obj.transform;
        Vector3 pos = transform.position;
        pos.x = targetpos.x + plusX;
        pos.y = targetpos.y + plusY;
        if(sameZ == true)
        {
            pos.z = targetpos.z;
        }
        transform.position = pos;
    }

    public void DeccelShake(int t, int maxt, int width, int dirX, int dirY, int speed, GameObject obj) //maxtとspeedは同じ倍数にする
    {
        if(t % speed == 0)
        {
            int T = t / speed;
            int maxT = maxt / speed;
            int posX;
            int posY;

            if (T == maxT)
            {
                posX = T * dirX;
                if(Mathf.Abs(posX) > width)
                {
                    posX = width / (posX/ posX);
                }
                posY = T * dirY;
                if (Mathf.Abs(posY) > width)
                {
                    posY = width / (posY / posY);
                }
            }
            else
            {
                int dir = -((maxT - T) % 2);
                if (dir == 0)
                {
                    dir = 1;
                }
                int firstposX = (T + 1) * dirX * dir;
                int secondposX = T * dirX * dir;
                int firstposY = (T + 1) * dirY * dir;
                int secondposY = T * dirY * dir;

                posX = firstposX + secondposX;
                if (Mathf.Abs(posX) > width * 2)
                {
                    posX = 2 * dir * width;
                }
                posY = firstposY + secondposY;
                if (Mathf.Abs(posY) > width * 2)
                {
                    posY = 2 * dir * width;
                }
            }

            Transform transform = obj.transform;
            Vector3 pos = transform.localPosition;
            pos.x += posX;
            pos.y += posY;
            transform.localPosition = pos;
        }
    }

    public void MoveLocalPosX(GameObject obj, float posX)
    {
        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.x = posX;
        transform.localPosition = pos;
    }

    public void MoveLocalPosY(GameObject obj, float posY)
    {
        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.y = posY;
        transform.localPosition = pos;
    }

    public void MoveLocalPos(GameObject obj, float posX, float posY)
    {
        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.x = posX;
        pos.y = posY;
        transform.localPosition = pos;
    }

    public void MoveLocalPosZ(GameObject obj, float posX, float posY, float posZ)
    {
        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.x = posX;
        pos.y = posY;
        pos.z = posZ;
        transform.localPosition = pos;
    }

    public void MoveLocalPosPlusX(GameObject obj, float plusX)
    {
        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.x += plusX;
        transform.localPosition = pos;
    }

    public void MoveLocalPosPlusY(GameObject obj, float plusY)
    {
        Transform transform = obj.transform;
        Vector3 pos = transform.localPosition;
        pos.y += plusY;
        transform.localPosition = pos;
    }

    public void MoveLocalScale(GameObject obj, float scaleX, float scaleY)
    {
        Transform transform = obj.transform;
        Vector3 scale = transform.localScale;
        scale.x = scaleX;
        scale.y = scaleY;
        transform.localScale = scale;
    }
}