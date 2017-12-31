using System.Threading.Tasks;

namespace ZulipAPI {

    public class Users : EndPointBase {

        public UserCollection UserCollection { get; set; }
        public ResponseUsers Response { get; set; }

        public Users(ZulipClient ZulipClient) {
            _ZulipClient = ZulipClient;
            _HttpClient = ZulipClient.Login();
        }

        public async Task GetUsersAsync() {
            await GetJsonAsStringAsync(EndPointPath.Users);
        }

        protected override void ParseResponse() {
            dynamic JObj = JSONHelper.ParseJSON(JsonOutput);
            Response = JSONHelper.ParseJObject<ResponseUsers>(JObj);

            if (Response.Result == "success") {
                UserCollection = new UserCollection();
                var result = JSONHelper.ParseJArray<User>(Response.Members);
                if (result != null) {
                    foreach (var user in result) {
                        this.UserCollection.Add(user);
                    }
                }
            } else {
                throw new FailedCallException("The API call returned with an error.") { ZulipServerResponse = Response };
            }
        }
    }
}
