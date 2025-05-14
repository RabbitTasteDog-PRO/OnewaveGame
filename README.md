
3thParty
UGS (세팅값이 필요합니다)
* 구글스프레드시트를 활용한 데이터 관리 및 load,write
* 도큐맨트 : https://shlifedev.gitbook.io/unitygooglesheets/additional/apps-script-backend-security
* https://shlifedev.gitbook.io/unitygooglesheets/additional/apps-script-backend-security
* 상단 HamsterLib ->  USG -> Setting -> 세팅완료 후 SAVE
    * 인정정보 세팅
        * 구글 스크립트 URL : https://script.google.com/macros/s/AKfycbzCO6LyJ1in5EJCAB5cCqUIybfQtB4nln9_65hyjFRYRP4yfkiMhCVa7yFNXBc-dVQS/exec
        * 스크립트 비밀번호 : 0000
        * 구글폴더 ID : 1glNxrlQQNxNaODjJawE9X3FdZcnF35Hf
DoTween 
* 간단한 애니메이션
UniTask
* 비동기 Task 

IDE
* VSCode

개발환경 
* 맥
* Unity 2022.3.45
* 빌드세팅 : Window,Mac,Linux


데이터 구글스프레드시트 
https://docs.google.com/spreadsheets/d/1DZ84eoZ9Bk6mKYqaau7GmhJBAtENH6K1J5FJtiSeiSY/edit?gid=0#gid=0


과제 간단설명 

블리츠블링크의 Q,W,E,R 스킬구현 
데이터는 적용되나 레벨링 미 구현으로 컴포넌트 또는 스크립트상에서 레벨을 조절하여 Log로 표시 


DataControllManager.cs 
- CVS로 데이터를 읽고 로드하는 매니저입니다 
- 미사용이나 팔요 시 사용합니다 
- SampleScene의 @DataManager입니다 


SkillManager.cs
- 구글 스프레드시트를 적용하여 스킬데이터를 읽고 스킬을 불러옵니다
    - 스크립터블오브젝트로 데이터를 시각화 하여 보여줍니다
    - 구글 스프레드시트와 연동하여 데이터 적용 시 갱신됩니다 
    - https://shlifedev.gitbook.io/unitygooglesheets/additional/apps-script-backend-security
        - 오픈소스 도큐멘트 입니다
- SampleScene의 @SkillManager입니다

Plyer 오브젝트 안에서  Q,W,E,R 스킬을 구현하였습니다.


확인부탁드립니다 
감사합니다.
