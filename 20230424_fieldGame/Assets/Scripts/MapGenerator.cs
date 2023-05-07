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
            //wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //wall.transform.localScale = new Vector3(1f, 2f, 1f);
            //wall.GetComponent<Renderer>().material.color = Color.grey;

            //OnDrawRectangle(0, 0, mapSize.x, mapSize.y); //던전 사이즈에 맞게 벽을 그림
            TreeNode rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y); //루트가 될 트리 생성
            DivideTree(rootNode, 0); //트리 분할
            GenerateDungeon(rootNode, 0); //방 생성
            //GenerateRoad(rootNode, 0); //길 연결
            OnDrawAll();
        }

        private void DivideTree(TreeNode treeNode, int n) //재귀 함수
        {
            if (n < maxNode) //0 부터 시작해서 노드의 최댓값에 이를 때 까지 반복
            {
                RectInt size = treeNode.treeSize; //이전 트리의 범위 값 저장, 사각형의 범위를 담기 위해 Rect 사용
                int length = size.width >= size.height ? size.width : size.height; //사각형의 가로와 세로 중 길이가 긴 축을, 트리를 반으로 나누는 기준선으로 사용
                int split = Mathf.RoundToInt(UnityEngine.Random.Range(length * minDivideSize, length * maxDivideSize)); //기준선 위에서 최소 범위와 최대 범위 사이의 값을 무작위로 선택
                if (size.width >= size.height) //가로
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, split, size.height); //기준선을 반으로 나눈 값인 split을 가로 길이로, 이전 트리의 height값을 세로 길이로 사용
                    treeNode.rightTree = new TreeNode(size.x + split, size.y, size.width - split, size.height); //x값에 split값을 더해 좌표 설정, 이전 트리의 width값에 split값을 빼 가로 길이 설정
                    if (n > 0) OnDrawLine(new Vector2(size.x + split, size.y), new Vector2(size.x + split, size.y + size.height)); //기준선 렌더링
                }
                else //세로
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, size.width, split);
                    treeNode.rightTree = new TreeNode(size.x, size.y + split, size.width, size.height - split);
                    if (n > 0) OnDrawLine(new Vector2(size.x, size.y + split), new Vector2(size.x + size.width, size.y + split));
                }
                treeNode.leftTree.parentTree = treeNode; //분할한 트리의 부모 트리를 매개변수로 받은 트리로 할당
                treeNode.rightTree.parentTree = treeNode;
                DivideTree(treeNode.leftTree, n + 1); //재귀 함수, 자식 트리를 매개변수로 넘기고 노드 값 1 증가 시킴
                DivideTree(treeNode.rightTree, n + 1);
            }
        }

        private RectInt GenerateDungeon(TreeNode treeNode, int n) //방 생성
        {
            if (n == maxNode) //노드가 최하위일 때만 조건문 실행
            {
                RectInt size = treeNode.treeSize;
                int width = Mathf.Max(UnityEngine.Random.Range(size.width / 2, size.width - 2)); //트리 범위 내에서 무작위 크기 선택, 최소 크기 : width / 2
                int height = Mathf.Max(UnityEngine.Random.Range(size.height / 2, size.height - 2));
                int x = treeNode.treeSize.x + UnityEngine.Random.Range(2, size.width - width); //최대 크기 : width / 2
                int y = treeNode.treeSize.y + UnityEngine.Random.Range(2, size.height - height);
                OnDrawDungeon(x, y, width, height); //던전 렌더링
                return new RectInt(x, y, width, height); //리턴 값은 던전의 크기로 길을 생성할 때 크기 정보로 활용
            }
            treeNode.leftTree.dungeonSize = GenerateDungeon(treeNode.leftTree, n + 1); //리턴 값 = 던전 크기
            treeNode.rightTree.dungeonSize = GenerateDungeon(treeNode.rightTree, n + 1);
            return treeNode.leftTree.dungeonSize; //부모 트리의 던전 크기는 자식 트리의 던전 크기 그대로 사용
        }

        //private void GenerateRoad(TreeNode treeNode, int n) //길 연결
        //{
        //    if (n == maxNode) return; //노드가 최하위일 때는 길을 연결하지 않음, 최하위 노드는 자식 트리가 없기 때문
        //    int x1 = GetCenterX(treeNode.leftTree.dungeonSize); //자식 트리의 던전 중앙 위치를 가져옴
        //    int x2 = GetCenterX(treeNode.rightTree.dungeonSize);
        //    int y1 = GetCenterY(treeNode.leftTree.dungeonSize);
        //    int y2 = GetCenterY(treeNode.rightTree.dungeonSize);
        //    for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++) //x1과 x2중 값이 작은 곳부터 값이 큰 곳까지 타일 생성
        //        //tilemap.SetTile(new Vector3Int(x - mapSize.x / 2, y1 - mapSize.y / 2, 0), tile); //mapSize.x / 2를 빼는 이유는 화면 중앙에 맞추기 위함
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

        private void OnDrawLine(Vector2 from, Vector2 to) //라인 렌더러를 이용해 라인을 그리는 메소드
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
                for (int i = x1 + 1; i <= x2; i++)
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

        }

        private void OnDrawDungeon(int x, int y, int width, int height) //크기에 맞춰 타일을 생성하는 메소드
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
            GameObject clone_box = Instantiate(box, new Vector3Int((x + x + width) / 2, 0, (y + y + height) / 2), Quaternion.identity) as GameObject;
            clone_box.transform.parent = transform;
            int rnd = UnityEngine.Random.Range(0, 3);
            if (rnd == 0) caveMap[x + width / 2, y + height - 1] = 0;
            else if (rnd == 1) caveMap[x + width - 1, y + height / 2] = 0;
            else if (rnd == 2) caveMap[x + width / 2, y] = 0;
            else if (rnd == 3) caveMap[x, y + height / 2] = 0;

            int rnd2 = UnityEngine.Random.Range(0, 3);
            if (rnd2 == 0) caveMap[x + width / 2, y + height - 1] = 0;
            else if (rnd2 == 1) caveMap[x + width - 1, y + height / 2] = 0;
            else if (rnd2 == 2) caveMap[x + width / 2, y] = 0;
            else if (rnd2 == 3) caveMap[x, y + height / 2] = 0;
        }
        private void OnDrawAll() //크기에 맞춰 타일을 생성하는 메소드
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
                        GameObject cube = Instantiate(wall, new Vector3Int(i, 0, j), Quaternion.identity) as GameObject;
                        cube.transform.parent = transform;
                    }
                }

        }

        //private void OnDrawRectangle(int x, int y, int width, int height) //라인 렌더러를 이용해 사각형을 그리는 메소드
        //{
        //    LineRenderer lineRenderer = Instantiate(rectangle, lineHolder).GetComponent<LineRenderer>();
        //    lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); //위치를 화면 중앙에 맞춤
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
