/* Loaad testing is primarily concerned with assessing the current performance of your system
in terms of concurrent users or requests per second.
When you understand if your system is meeting the performance goals, this is the type of test
you'll run.

Run a load test to:
- Access the current performance of your system under typical and peak load
- Make sure you are continuously meeting the performance standards as you make changesto your system

Can be used to simulate a normal day in your business
Load tests could be a part of your CI/CD pipelines
*/

import { sleep } from 'k6';
import http from 'k6/http';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '5m', target: 100 }, // simulate ramp-up of traffic from 1 to 100 users over 5 mins
        { duration: '10m', target: 100 }, // stay at 100 users for 10 mins
        { duration: '5m', target: 0 }, // ramp-down to 0 users
    ],
    thresholds:{
        http_req_duration: ['p(99)<150'], // 99% of requests must complete below 150s
    }
};

export default () => {

    let response = http.get('https://localhost:5001/api/meetup?pageSize=50&pageNumber=1');

    sleep(1);
};