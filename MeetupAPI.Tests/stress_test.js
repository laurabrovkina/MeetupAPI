import { sleep } from 'k6';
import http from 'k6/http';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '2m', target: 100 }, // below normal load, it will increase number of users from 0 to 100 at the first 2 mims
        { duration: '5m', target: 100 }, // then 5 mins it stays with the same load of 100 users
        { duration: '2m', target: 200 }, // it will increase from 100 to 200 users in another 2 mins
        { duration: '5m', target: 200 },
        { duration: '2m', target: 300 }, // around breaking point
        { duration: '5m', target: 300 },
        { duration: '2m', target: 400 }, // beyond the breaking point
        { duration: '5m', target: 400 },
        { duration: '10m', target: 0 }, // scale down. Recovery stage.
    ],
};

const API_BASE_URL = 'https://localhost:5001';
const name = '4Dev';

export default () => {
    http.batch([
        ['GET', `${API_BASE_URL}/api/meetup?pageSize=50&pageNumber=1`],
        ['GET', `${API_BASE_URL}/api/meetup/${name}`],
    ]);

    sleep(1);
};