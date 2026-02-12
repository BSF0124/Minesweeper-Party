using UnityEngine;
using UnityEngine.UI;

/* MainMenuController
 * 메인 메뉴 화면에서
 * - 게임 모드 선택
 * - 난이도 선택
 * - 게임 씬 진입
 * 을 제어하는 UI 컨트롤러
 */
public class MainMenuController : MonoBehaviour
{
    // 협동 모드 버튼 클릭 처리
    public void BtnClickCoop()
    {
        Debug.Log("Play Coop Mode");
    }

    // 대전 모드 버튼 클릭 처리
    public void BtnClickVersus()
    {
        Debug.Log("Play Versus Mode");
    }

    // 쉬움 난이도로 클래식 게임 모드 시작
    public void BtnClickEasy()
    {
        GameStartContext.SetClassic(Difficulty.Easy);
        Utils.LoadScene(SceneNames.Game);
    }

    // 보통 난이도로 클래식 게임 모드 시작
    public void BtnClickNormal()
    {
        GameStartContext.SetClassic(Difficulty.Normal);
        Utils.LoadScene(SceneNames.Game);
    }

    // 어려움 난이도로 클래식 게임 모드 시작
    public void BtnClickHard()
    {
        GameStartContext.SetClassic(Difficulty.Hard);
        Utils.LoadScene(SceneNames.Game);
    }
}
