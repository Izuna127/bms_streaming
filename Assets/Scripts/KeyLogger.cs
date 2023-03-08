using System.Collections.Generic;
using UnityEngine;

public class KeyLogger : MonoBehaviour
{
    private bool _isScratchActive = false;
    private Vector2 _oldScratchPos = new(2f, 2f);
    private Vector2 _initScratchPos = new(2f, 2f);

    private void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _oldScratchPos = BmsTool.I.Scratch.ReadValue();
        _initScratchPos = _oldScratchPos;
    }

    // Update is called once per frame
    private void Update()
    {
        // TODO: beatoraja��State��PLAY�̏ꍇ�����J�E���g

        // ���Ղ̉񐔃J�E���g
        for (int i = 0; i < 7; i++)
        {
            if (BmsTool.I.ButtonControls[i].isPressed)
            {
                if (!Status[i + 2])
                {
                    Status[i + 2] = true;
                    TotalKeyPressed++;
                    TodayCounts[i + 2] += 1;
                }
            }
            else if (!BmsTool.I.ButtonControls[i].isPressed && Status[i + 2])
            {
                Status[i + 2] = false;
            }
        }

        // �M�̉񐔃J�E���g
        // �����̊� -> ����ԁF(-0.71,0.71)�B���v���F(-0.01,1.00)�B�����v���F(0.00,1.00)�ۂ�
        Vector2 currentScratchPos = BmsTool.I.Scratch.ReadValue();
        if (_oldScratchPos.x != currentScratchPos.x && currentScratchPos.y != _initScratchPos.y)
        {
            _isScratchActive = true;
            _oldScratchPos = currentScratchPos;
            TotalKeyPressed++;

            // �����̉񐔃J�E���g�p����
            if (currentScratchPos.x < 0.00f) // x���}�C�i�X�̏ꍇ�͎��v���(�̂͂�)
            {
                TodayCounts[1] += 1;
                Status[0] = false;
                Status[1] = true;
            }
            else
            {
                TodayCounts[0] += 1;
                Status[0] = true;
                Status[1] = false;
            }
        }

        if (currentScratchPos == _initScratchPos && _isScratchActive)
        {
            _isScratchActive = false;
            _oldScratchPos = currentScratchPos;
            Status[0] = false;
            Status[1] = false;
        }
    }

    public static KeyLogger I { get; private set; }

    public int TotalKeyPressed { get; set; }

    // ���ՂƎM�̏�ԕێ��p �M���A�M���A1�`7�̏�
    public List<bool> Status { get; } = new(9)
    {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
    };

    // �e���ՂƎM�̑Ō���(Today) �M���A�M���A1�`7�̏�
    public List<int> TodayCounts { get; } = new(9) { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
}
