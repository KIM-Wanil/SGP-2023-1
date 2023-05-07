using UnityEngine;
using System.Linq;


namespace DungeonGeneratorByBinarySpacePartitioning
{
    public class TreeNode
    {
        public TreeNode leftTree;
        public TreeNode rightTree;
        public TreeNode parentTree;
        public RectInt treeSize;
        public RectInt dungeonSize;

        public TreeNode(int x, int y, int width, int height)
        {
            treeSize.x = x;
            treeSize.y = y;
            treeSize.width = width;
            treeSize.height = height;
        }
    }

    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int mapSize;

        [SerializeField] private int maxNode;
        [SerializeField] private float minDivideSize;
        [SerializeField] private float maxDivideSize;
        [SerializeField] private int minRoomSize;

        [SerializeField] private GameObject line;
        [SerializeField] private Transform lineHolder;
        [SerializeField] private GameObject rectangle;

        private GameObject wall;
        [SerializeField] private GameObject box;
        [SerializeField] private GameObject altar;
        [SerializeField] private GameObject hideout;
        private int[,] caveMap;

        private void Awake()
        {
            caveMap = new int[mapSize.x + 1, mapSize.y + 1];
            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    if (i == 0 || i == mapSize.x - 1 || j == 0 || j == 0 + mapSize.y - 1) caveMap[i, j] = 1;
                    else caveMap[i, j] = 0;
                }
            }
            wall = Resources.Load<GameObject>("Prefabs/Wall");
            box = Resources.Load<GameObject>("Prefabs/Box");
            altar = Resources.Load<GameObject>("Prefabs/Altar");
            hideout = Resources.Load<GameObject>("Prefabs/Hideout");
            //wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //wall.transform.localScale = new Vector3(1f, 2f, 1f);
            //wall.GetComponent<Renderer>().material.color = Color.grey;

            //OnDrawRectangle(0, 0, mapSize.x, mapSize.y); //���� ����� �°� ���� �׸�
            TreeNode rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y); //��Ʈ�� �� Ʈ�� ����
            DivideTree(rootNode, 0); //Ʈ�� ����
            GenerateDungeon(rootNode, 0); //�� ����
            //GenerateRoad(rootNode, 0); //�� ����
            OnDrawAll();
        }

        private void DivideTree(TreeNode treeNode, int n) //��� �Լ�
        {
            if (n < maxNode) //0 ���� �����ؼ� ����� �ִ񰪿� �̸� �� ���� �ݺ�
            {
                RectInt size = treeNode.treeSize; //���� Ʈ���� ���� �� ����, �簢���� ������ ��� ���� Rect ���
                int length = size.width >= size.height ? size.width : size.height; //�簢���� ���ο� ���� �� ���̰� �� ����, Ʈ���� ������ ������ ���ؼ����� ���
                int split = Mathf.RoundToInt(UnityEngine.Random.Range(length * minDivideSize, length * maxDivideSize)); //���ؼ� ������ �ּ� ������ �ִ� ���� ������ ���� �������� ����
                if (size.width >= size.height) //����
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, split, size.height); //���ؼ��� ������ ���� ���� split�� ���� ���̷�, ���� Ʈ���� height���� ���� ���̷� ���
                    treeNode.rightTree = new TreeNode(size.x + split, size.y, size.width - split, size.height); //x���� split���� ���� ��ǥ ����, ���� Ʈ���� width���� split���� �� ���� ���� ����
                    if (n > 0) OnDrawLine(new Vector2(size.x + split, size.y), new Vector2(size.x + split, size.y + size.height)); //���ؼ� ������
                }
                else //����
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, size.width, split);
                    treeNode.rightTree = new TreeNode(size.x, size.y + split, size.width, size.height - split);
                    if (n > 0) OnDrawLine(new Vector2(size.x, size.y + split), new Vector2(size.x + size.width, size.y + split));
                }
                treeNode.leftTree.parentTree = treeNode; //������ Ʈ���� �θ� Ʈ���� �Ű������� ���� Ʈ���� �Ҵ�
                treeNode.rightTree.parentTree = treeNode;
                DivideTree(treeNode.leftTree, n + 1); //��� �Լ�, �ڽ� Ʈ���� �Ű������� �ѱ�� ��� �� 1 ���� ��Ŵ
                DivideTree(treeNode.rightTree, n + 1);
            }
        }

        private RectInt GenerateDungeon(TreeNode treeNode, int n) //�� ����
        {
            if (n == maxNode) //��尡 �������� ���� ���ǹ� ����
            {
                RectInt size = treeNode.treeSize;
                int width = Mathf.Max(UnityEngine.Random.Range(size.width / 2, size.width - 2)); //Ʈ�� ���� ������ ������ ũ�� ����, �ּ� ũ�� : width / 2
                int height = Mathf.Max(UnityEngine.Random.Range(size.height / 2, size.height - 2));
                int x = treeNode.treeSize.x + UnityEngine.Random.Range(2, size.width - width); //�ִ� ũ�� : width / 2
                int y = treeNode.treeSize.y + UnityEngine.Random.Range(2, size.height - height);
                OnDrawDungeon(x, y, width, height); //���� ������
                return new RectInt(x, y, width, height); //���� ���� ������ ũ��� ���� ������ �� ũ�� ������ Ȱ��
            }
            treeNode.leftTree.dungeonSize = GenerateDungeon(treeNode.leftTree, n + 1); //���� �� = ���� ũ��
            treeNode.rightTree.dungeonSize = GenerateDungeon(treeNode.rightTree, n + 1);
            return treeNode.leftTree.dungeonSize; //�θ� Ʈ���� ���� ũ��� �ڽ� Ʈ���� ���� ũ�� �״�� ���
        }

        //private void GenerateRoad(TreeNode treeNode, int n) //�� ����
        //{
        //    if (n == maxNode) return; //��尡 �������� ���� ���� �������� ����, ������ ���� �ڽ� Ʈ���� ���� ����
        //    int x1 = GetCenterX(treeNode.leftTree.dungeonSize); //�ڽ� Ʈ���� ���� �߾� ��ġ�� ������
        //    int x2 = GetCenterX(treeNode.rightTree.dungeonSize);
        //    int y1 = GetCenterY(treeNode.leftTree.dungeonSize);
        //    int y2 = GetCenterY(treeNode.rightTree.dungeonSize);
        //    for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++) //x1�� x2�� ���� ���� ������ ���� ū ������ Ÿ�� ����
        //        //tilemap.SetTile(new Vector3Int(x - mapSize.x / 2, y1 - mapSize.y / 2, 0), tile); //mapSize.x / 2�� ���� ������ ȭ�� �߾ӿ� ���߱� ����
        //        {
        //            GameObject cube = Instantiate(wall, new Vector3Int(x - mapSize.x / 2, y1 - mapSize.y / 2, 0), Quaternion.identity) as GameObject;
        //            cube.transform.parent = transform;
        //        }
        //    for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
        //        //tilemap.SetTile(new Vector3Int(x2 - mapSize.x / 2, y - mapSize.y / 2, 0), tile);
        //        {
        //            GameObject cube = Instantiate(wall, new Vector3Int(x2 - mapSize.x / 2, y - mapSize.y / 2, 0), Quaternion.identity) as GameObject;
        //            cube.transform.parent = transform;
        //        }
        //    if(n<= maxNode-2)
        //    {

        //    }
        //    GenerateRoad(treeNode.leftTree, n + 1);
        //    GenerateRoad(treeNode.rightTree, n + 1);
        //}

        private void OnDrawLine(Vector2 from, Vector2 to) //���� �������� �̿��� ������ �׸��� �޼ҵ�
        {
            //LineRenderer lineRenderer = Instantiate(line, lineHolder).GetComponent<LineRenderer>();
            //lineRenderer.SetPosition(0, from - mapSize / 2);
            //lineRenderer.SetPosition(1, to - mapSize / 2);
            int x1 = (int)from.x;
            int x2 = (int)to.x;
            int y1 = (int)from.y;
            int y2 = (int)to.y;

            int rnd = UnityEngine.Random.Range(0, 4);
            
            Debug.Log(rnd);
            if (x2 > x1)
            {
                for (int i = x1; i <= x2; i++)
                {
                    caveMap[i, y2] = 1;
                }
                if (rnd == 0)
                {
                    for (int i = x1 + 1; i <= (x1 + x2) / 2; i++)
                    {
                        caveMap[i, y2] = 0;
                    }
                }
                else if (rnd == 1)
                {
                    for (int i = (x1 + x2) / 2; i <= x2 - 1; i++)
                    {
                        caveMap[i, y2] = 0;
                    }
                }
                else if (rnd == 2)
                {
                    for (int i = (x1 + x2) / 2 - 1; i <= (x1 + x2) / 2 + 1; i++)
                    {
                        caveMap[i, y2] = 0;
                    }
                }
                else if (rnd == 3)
                {
                    for (int i = x1 + 1; i <= (x1 + (x1 + x2) / 2) / 2; i++)
                    {
                        caveMap[i, y2] = 0;
                    }
                    for (int i = (x2 + (x1 + x2) / 2) / 2; i <= x2 - 1; i++)
                    {
                        caveMap[i, y2] = 0;
                    }
                }
            }
            else if (y2 > y1)
            {
                for (int i = y1; i <= y2; i++)
                {
                    caveMap[x2, i] = 1;
                }
                if (rnd == 0)
                {
                    for (int i = y1 + 1; i <= (y1 + y2) / 2; i++)
                    {
                        caveMap[x2, i] = 0;
                    }
                }
                else if (rnd == 1)
                {
                    for (int i = (y1 + y2) / 2; i <= y2 - 1; i++)
                    {
                        caveMap[x2, i] = 0;
                    }
                }
                else if (rnd == 2)
                {
                    for (int i = (y1 + y2) / 2 - 1; i <= (y1 + y2) / 2 + 1; i++)
                    {
                        caveMap[x2, i] = 0;
                    }
                }
                else if (rnd == 3)
                {
                    for (int i = y1 + 1; i <= (y1 + (y1 + y2) / 2) / 2; i++)
                    {
                        caveMap[x2, i] = 0;
                    }
                    for (int i = (y2 + (y1 + y2) / 2) / 2; i <= y2 - 1; i++)
                    {
                        caveMap[x2, i] = 0;
                    }
                }
            }
            if (rnd == 2 || rnd == 3) return;
            Vector2Int hidePos = new Vector2Int(0, 0);
            hidePos.x = (x1 + x2) / 2+1;
            hidePos.y = (y1 + y2) / 2 +1;
            if (hidePos.x <= 0) hidePos.x = 1;
            else if (hidePos.x >= mapSize.x - 1) hidePos.x = mapSize.x - 2;
            if (hidePos.y <= 0) hidePos.y = 1;
            else if (hidePos.y >= mapSize.y - 1) hidePos.y = mapSize.y - 2;
            caveMap[hidePos.x, hidePos.y] = 3;

        }

        private void OnDrawDungeon(int x, int y, int width, int height) //ũ�⿡ ���� Ÿ���� �����ϴ� �޼ҵ�
        {
            for (int i = x; i < x + width; i++)
                for (int j = y; j < y + height; j++)
                //tilemap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), tile);
                {
                    if (i == x || i == x + width - 1 || j == y || j == y + height - 1)
                    {
                        //GameObject cube = Instantiate(wall, new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), Quaternion.identity) as GameObject;
                        //cube.transform.parent = transform;
                        caveMap[i, j] = 1;
                    }
                }
            caveMap[x+width/2, y+height/2] = 2;
            //GameObject clone_box = Instantiate(box, new Vector3((x + x + width) / 2, 0.5f, (y + y + height) / 2), Quaternion.identity) as GameObject;
            //clone_box.transform.parent = transform;
            int rnd = UnityEngine.Random.Range(0, 4);
            if (rnd == 0) caveMap[x + width / 2, y + height - 1] = 0;
            else if (rnd == 1) caveMap[x + width - 1, y + height / 2] = 0;
            else if (rnd == 2) caveMap[x + width / 2, y] = 0;
            else if (rnd == 3) caveMap[x, y + height / 2] = 0;

            int rnd2 = UnityEngine.Random.Range(0, 4);
            if (rnd2 == 0) caveMap[x + width / 2, y + height - 1] = 0;
            else if (rnd2 == 1) caveMap[x + width - 1, y + height / 2] = 0;
            else if (rnd2 == 2) caveMap[x + width / 2, y] = 0;
            else if (rnd2 == 3) caveMap[x, y + height / 2] = 0;

            
        }
        private void OnDrawAll() //ũ�⿡ ���� Ÿ���� �����ϴ� �޼ҵ�
        {
            for (int i = mapSize.x / 2 - 4; i <= mapSize.x / 2 + 4; i++)
                for (int j = mapSize.y / 2 - 4; j <= mapSize.y / 2 + 4; j++)
                    caveMap[i, j] = 0;
            GameObject clone_altar = Instantiate(altar, new Vector3Int(mapSize.x / 2, 0, mapSize.y / 2), Quaternion.identity) as GameObject;
            clone_altar.transform.parent = transform;

            for (int i = 0; i < mapSize.x; i++)
                for (int j = 0; j < mapSize.y; j++)
                //tilemap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), tile);
                {
                    if (caveMap[i, j] == 1)
                    {
                        GameObject clone_cube = Instantiate(wall, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        clone_cube.transform.parent = transform;
                    }
                    else if (caveMap[i, j] == 2)
                    {
                        GameObject clone_box = Instantiate(box, new Vector3(i, 0.5f, j), Quaternion.identity) as GameObject;
                        clone_box.transform.parent = transform;
                    }
                    else if (caveMap[i, j] == 3)
                    {
                        GameObject clone_hideout = Instantiate(hideout, new Vector3(i, 0f, j), Quaternion.identity) as GameObject;
                        clone_hideout.transform.parent = transform;
                    }
                    
                }

        }

        //private void OnDrawRectangle(int x, int y, int width, int height) //���� �������� �̿��� �簢���� �׸��� �޼ҵ�
        //{
        //    LineRenderer lineRenderer = Instantiate(rectangle, lineHolder).GetComponent<LineRenderer>();
        //    lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); //��ġ�� ȭ�� �߾ӿ� ����
        //    lineRenderer.SetPosition(1, new Vector2(x + width, y) - mapSize / 2);
        //    lineRenderer.SetPosition(2, new Vector2(x + width, y + height) - mapSize / 2);
        //    lineRenderer.SetPosition(3, new Vector2(x, y + height) - mapSize / 2);
        //}

        private int GetCenterX(RectInt size)
        {
            return size.x + size.width / 2;
        }

        private int GetCenterY(RectInt size)
        {
            return size.y + size.height / 2;
        }
    }
}
